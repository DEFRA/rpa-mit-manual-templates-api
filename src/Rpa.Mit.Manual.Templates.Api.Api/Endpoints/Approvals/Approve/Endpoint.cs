using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoice
{
    /// <summary>
    /// approve an AP invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : EndpointWithMapping<ApproveInvoiceRequest, ApproveInvoiceResponse, InvoiceApproval>
    {
        private readonly PaymentHub _options;
        private readonly IApprovalsRepo _iApprovalsRepo;

        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            IOptions<PaymentHub> options,

            ILogger<ApproveInvoiceEndpoint> logger,
            IApprovalsRepo iApprovalsRepo,
            IServiceBusProvider iServiceBusProvider,
            IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _options = options.Value;
            _logger = logger;

            _iApprovalsRepo = iApprovalsRepo;
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

            if (string.IsNullOrEmpty(_options.CONNECTION) || string.IsNullOrEmpty(_options.TOPIC))
            {
                response.Result = false;
                response.Message = "No values for Servicebus connection given.";
                await SendAsync(response, 400, cancellation: ct);
            }

            try
            {
                InvoiceApproval approval = await MapToEntityAsync(r, ct);

                if (await _iApprovalsRepo.ApproveInvoice(approval, ct))
                {
                    // get the invoice requests and lines for sending to payment hub
                    var invoiceRequests = await _iApprovalsRepo.GetInvoiceRequestsForAzure(r.Id, ct);
                    int idx = 0;

                    foreach (InvoiceRequestForAzure request in invoiceRequests)
                    {
                        // create the json
                        var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson<InvoiceRequestForAzure>(request, ct);

                        if (string.IsNullOrEmpty(invoiceRequestJson))
                        {
                            response.Result = false;
                            response.Message += "Error creating payment hub json for invoice request " + request.InvoiceRequestId;
                        }
                        else
                        {
                            await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestJson);
                            idx++;
                        }
                    }

                    if (idx == invoiceRequests.Count())
                    {
                        response.Message += "All invoices approved and data sent to Payment Hub.";
                    }
                }
                else
                {
                    response.Result = false;
                    response.Message = "Error approving invoices. None sent to payment hub.";
                    await SendAsync(response, 400, cancellation: ct);
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

        public sealed override async Task<InvoiceApproval> MapToEntityAsync(ApproveInvoiceRequest r, CancellationToken ct = default)
        {
            var invoiceApproval = await Task.FromResult(new InvoiceApproval());

            invoiceApproval.ApproverEmail = User.Identity?.Name!;
            invoiceApproval.DateApproved = DateTime.UtcNow;
            invoiceApproval.Id = r.Id;

            return invoiceApproval;
        }
    }
}