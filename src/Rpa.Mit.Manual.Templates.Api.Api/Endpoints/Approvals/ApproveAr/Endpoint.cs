using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoiceAr
{
    /// <summary>
    /// approve an AR invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceArEndpoint : EndpointWithMapping<ApproveInvoiceArRequest, ApproveInvoiceArResponse, InvoiceApproval>
    {
        private readonly PaymentHub _options;
        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<ApproveInvoiceArEndpoint> _logger;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;

        public ApproveInvoiceArEndpoint(
                                        IOptions<PaymentHub> options,
                                        IApprovalsRepo iApprovalsRepo,
                                        IServiceBusProvider iServiceBusProvider,
                                        ILogger<ApproveInvoiceArEndpoint> logger,
                                        IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _options = options.Value;
            _iApprovalsRepo = iApprovalsRepo;
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
                InvoiceApproval mappedApproval = await MapToEntityAsync(r, ct);

                if (await _iApprovalsRepo.ApproveInvoice(mappedApproval, ct))
                {
                    // get the invoice requests and lines for sending to payment hub
                    var invoiceRequestsForAzure = await _iApprovalsRepo.GetInvoiceRequestsForAzure(r.Id, ct);
                    int idx = 0;

                    foreach (InvoiceRequestForAzure request in invoiceRequestsForAzure)
                    {
                        // create the json
                        var invoiceRequestForAzureJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson(request, ct);

                        if (string.IsNullOrEmpty(invoiceRequestForAzureJson))
                        {
                            response.Result = false;
                            response.Message += "Error creating payment hub json with invoice request " + request.InvoiceRequestId;
                        }
                        else
                        {
                            await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestForAzureJson);
                            idx++;
                        }
                    }

                    if (idx == invoiceRequestsForAzure.Count())
                    {
                        response.Message += "All invoices approved and data sent to Payment Hub.";
                    }
                }
                else
                {
                    response.Result = false;
                    response.Message = "Error approving invoices. Nothimng has been sent to the payment hub.";
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

        public sealed override async Task<InvoiceApproval> MapToEntityAsync(ApproveInvoiceArRequest r, CancellationToken ct = default)
        {
            var mappedInvoiceApproval = await Task.FromResult(new InvoiceApproval());

            mappedInvoiceApproval.ApproverEmail = User.Identity?.Name!;
            mappedInvoiceApproval.DateApproved = DateTime.UtcNow;
            mappedInvoiceApproval.Id = r.Id;

            return mappedInvoiceApproval;
        }
    }
}