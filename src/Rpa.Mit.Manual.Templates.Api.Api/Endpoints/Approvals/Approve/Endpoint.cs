using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
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
        private readonly PaymentHub _options;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;

        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            IOptions<PaymentHub> options,

            ILogger<ApproveInvoiceEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo,
            IServiceBusProvider iServiceBusProvider,
            IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _options = options.Value;
            _logger = logger;

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

            ApproveInvoiceResponse response = new();
            response.Result = true;

            try
            {
                if (string.IsNullOrEmpty(_options.CONNECTION) || string.IsNullOrEmpty(_options.TOPIC))
                {
                    ThrowError("No values for Servicebus connection given.!");
                }

                // get the invoice requests and lines for sending to payment hub
                var invoiceRequests = await _iInvoiceRequestRepo.GetInvoiceRequestsForAzure(r.Id, ct);

                int idx = 0;
                List<string> approvals = new List<string>();

                foreach (InvoiceRequestForAzure request in invoiceRequests)
                {
                    // create the json
                    var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson<InvoiceRequestForAzure>(request, ct);

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

                if (idx == invoiceRequests.Count())
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