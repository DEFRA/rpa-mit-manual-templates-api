using System.Diagnostics.CodeAnalysis;
using System.Text;

using Rpa.Mit.Manual.Templates.Api.Api.Services;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoice
{
    /// <summary>
    /// approve an AP invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : Endpoint<ApproveInvoiceRequest, ApproveInvoiceResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly IEmailService _iEmailService;
        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            ILogger<ApproveInvoiceEndpoint> logger,
            IEmailService iEmailService,
            IInvoiceRequestRepo iInvoiceRequestRepo,
            IServiceBusProvider iServiceBusProvider,
            IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _iEmailService = iEmailService;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
            _iServiceBusProvider = iServiceBusProvider;
            _iPaymentHubJsonGenerator = iPaymentHubJsonGenerator;
        }

        public override void Configure()
        {
            //TODO: need to restrict this ep by role/policy
            Post("/approvals/approve");
        }

        public override async Task HandleAsync(ApproveInvoiceRequest r, CancellationToken ct)
        {
            var userEmail = User.Identity?.Name!;

            if (string.IsNullOrEmpty(userEmail))
            {
                ThrowError("Unable to identify approver");
            }
                
            ApproveInvoiceResponse response = new()
            {
                Result = true
            };

            StringBuilder sbErrors = new();

            try
            {
                // get the invoice requests and lines for sending to payment hub
                var invoiceRequests = await _iInvoiceRequestRepo.GetInvoiceRequestsForAzure(r.Id, ct);

                List<string> approvals = [];
                List<string> failures = [];

                foreach (InvoiceRequestForAzure request in invoiceRequests)
                {
                    // create the json
                    var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson<InvoiceRequestForAzure>(request, ct);

                    if (string.IsNullOrEmpty(invoiceRequestJson))
                    {
                        response.Result = false;
                        failures.Add(request.InvoiceRequestId);

                        sbErrors.AppendLine("Error creating payment hub json for invoice request " + request.InvoiceRequestId);
                    }
                    else
                    {
                        // TODO: need to properly handle failure here
                        if (await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestJson))
                        {
                            approvals.Add(request.InvoiceRequestId);
                        }
                        else
                        {
                            failures.Add(request.InvoiceRequestId);
                            sbErrors.AppendFormat("Error sending json for Invoice Request {0} to Payment Hub", request.InvoiceRequestId);
                        }
                    }
                }

                // now update our db with the results of approval
                await _iInvoiceRequestRepo.UpdateInvoiceRequestApprovalStatus(approvals, r.Id, userEmail, ct);

                if (approvals.Count == invoiceRequests.Count())
                {
                    sbErrors.AppendLine("All invoices approved and data sent to Payment Hub.");
                }
                else
                {
                    // TODO: email originator with this list of failed invoice requests. Note that failure is due to error(s) when sending to payment hub, not due to data errors.
                    await _iEmailService.EmailTransmissionFailure("aylmer.carson.external@eviden.com", failures, ct);

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