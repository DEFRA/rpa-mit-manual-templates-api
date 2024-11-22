using System.Diagnostics.CodeAnalysis;
using System.Text;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoiceAr
{
    /// <summary>
    /// approve an AR invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceArEndpoint : Endpoint<ApproveInvoiceArRequest, ApproveInvoiceArResponse>
    {
        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<ApproveInvoiceArEndpoint> _logger;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;

        public ApproveInvoiceArEndpoint(
                                        IInvoiceRequestRepo iInvoiceRequestRepo,
                                        IServiceBusProvider iServiceBusProvider,
                                        ILogger<ApproveInvoiceArEndpoint> logger,
                                        IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
            _iPaymentHubJsonGenerator = iPaymentHubJsonGenerator;
            _iServiceBusProvider = iServiceBusProvider;
        }

        public override void Configure()
        {
            Post("/approvals/approvear");
        }

        public override async Task HandleAsync(ApproveInvoiceArRequest r, CancellationToken ct)
        {
            var userEmail = User.Identity?.Name!;

            if (string.IsNullOrEmpty(userEmail))
            {
                ThrowError("Unable to identify approver");
            }

            StringBuilder sbErrors = new();

            ApproveInvoiceArResponse response = new()
            {
                Result = true
            };

            try
            {
                // get the AR invoice requests and lines for sending to payment hub
                var invoiceRequestsForAzure = await _iInvoiceRequestRepo.GetInvoiceRequestsArForAzure(r.Id, ct);
                int idx = 0;

                List<string> approvals = [];
                List<string> failures = [];

                foreach (InvoiceRequestArForAzure request in invoiceRequestsForAzure)
                {
                    // create the json
                    var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson<InvoiceRequestArForAzure>(request, ct);

                    if (string.IsNullOrEmpty(invoiceRequestJson))
                    {
                        response.Result = false;
                        failures.Add(request.InvoiceRequestId);

                        sbErrors.AppendLine("Error creating payment hub json for invoice request " + request.InvoiceRequestId);
                    }
                    else
                    {
                        if (await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestJson))
                        {
                            approvals.Add(request.InvoiceRequestId);
                            idx++;
                        }
                        else
                        {
                            failures.Add(request.InvoiceRequestId);
                            sbErrors.AppendFormat("Error sending json for Invoice Request {0} to Payment Hub", request.InvoiceRequestId);
                        }
                    }
                }

                // now update our db with the results of approval
                await _iInvoiceRequestRepo.UpdateInvoiceRequestApprovalStatus(approvals, r.Id, User.Identity?.Name!, ct);

                if (idx == invoiceRequestsForAzure.Count())
                {
                    sbErrors.AppendLine("All invoices approved and data sent to Payment Hub.");
                }
                else
                {
                    // TODO: email originator with this list of failed invoice requests. Note that failure is due to error(s) when sending to payment hub, not due to data errors.

                    sbErrors.AppendLine("Some invoices failed approval. The list of failures is here and the rest have been approved and sent to the Payment Hub.");
                }

                response.Message = sbErrors.ToString();

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}