using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
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
        private readonly PaymentHub _options;
        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<ApproveInvoiceArEndpoint> _logger;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;

        public ApproveInvoiceArEndpoint(
                                        IOptions<PaymentHub> options,
                                         IInvoiceRequestRepo iInvoiceRequestRepo,
                                        IServiceBusProvider iServiceBusProvider,
                                        ILogger<ApproveInvoiceArEndpoint> logger,
                                        IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _options = options.Value;
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

            ApproveInvoiceArResponse response = new();
            response.Result = true;

            if (string.IsNullOrEmpty(_options.CONNECTION) || string.IsNullOrEmpty(_options.TOPIC))
            {
                response.Result = false;
                response.Message = "No values for Servicebus connection given.";
                await SendAsync(response, 400, cancellation: ct);
            }

            try
            {
                if (string.IsNullOrEmpty(_options.CONNECTION) || string.IsNullOrEmpty(_options.TOPIC))
                {
                    ThrowError("No values for Servicebus connection given.!");
                }

                // get the AR invoice requests and lines for sending to payment hub
                var invoiceRequestsForAzure = await _iInvoiceRequestRepo.GetInvoiceRequestsArForAzure(r.Id, ct);
                int idx = 0;
                List<string> approvals = new List<string>();

                foreach (InvoiceRequestArForAzure request in invoiceRequestsForAzure)
                {
                    // create the json
                    var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson<InvoiceRequestArForAzure>(request, ct);

                    if (string.IsNullOrEmpty(invoiceRequestJson))
                    {
                        response.Result = false;
                        response.Message += "Error creating payment hub json for invoice request " + request.InvoiceRequestId + "||";
                    }
                    else
                    {
                        await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestJson);
                        approvals.Add(request.InvoiceRequestId);
                        idx++;
                    }
                }

                // now update our db with the results of approval
                await _iInvoiceRequestRepo.UpdateInvoiceRequestApprovalStatus(approvals, r.Id, User.Identity?.Name!, ct);

                if (idx == invoiceRequestsForAzure.Count())
                {
                    response.Message += "All invoices approved and data sent to Payment Hub.";
                }
                else
                {
                    response.Message += "Some invoices failed approval. The list of failures is here and the rest have been approved and sent to the Payment Hub.";
                }

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