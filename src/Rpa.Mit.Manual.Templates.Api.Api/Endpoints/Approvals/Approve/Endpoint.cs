using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.MitAzure;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : EndpointWithMapping<ApproveInvoiceRequest, ApproveInvoiceResponse, InvoiceApproval>
    {
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly IServiceBusProvider _iServiceBusProvider;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            ILogger<ApproveInvoiceEndpoint> logger, 
            IApprovalsRepo iApprovalsRepo,
            IServiceBusProvider iServiceBusProvider,    
            IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _iApprovalsRepo = iApprovalsRepo;
            _iServiceBusProvider = iServiceBusProvider;
            _iPaymentHubJsonGenerator = iPaymentHubJsonGenerator;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/approvals/approve");
        }

        public override async Task HandleAsync(ApproveInvoiceRequest r, CancellationToken ct)
        {
            ApproveInvoiceResponse response = new();
            response.Result = false;

            try
            {
                InvoiceApproval approval = await MapToEntityAsync(r, ct);

                if (await _iApprovalsRepo.ApproveInvoice(approval, ct))
                {
                    // get the invoice requests and lines for sending to payment hub
                    var invoiceRequests = await _iApprovalsRepo.GetInvoiceRequestsForAzure(r.Id,  ct);

                    foreach (InvoiceRequestForAzure request in invoiceRequests)
                    {
                        // create the json
                        var invoiceRequestJson = _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson(request, ct);

                        if (invoiceRequestJson.Length > 0)
                        {
                            // send correctly structured json to paymnent hub
                            await _iServiceBusProvider.SendInvoiceRequestJson(invoiceRequestJson);


                            response.Result = true;
                        }
                        else
                        {
                            response.Result = false;
                        }
                    }

                    response.Message = "Invoice approved and data sent to Payment Hub.";
                }
                else
                {
                    response.Message = "Error approving invoice.";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }

        public sealed override async Task<InvoiceApproval> MapToEntityAsync(ApproveInvoiceRequest r, CancellationToken ct = default)
        {
            var invoiceApproval = await Task.FromResult(new InvoiceApproval());

            invoiceApproval.ApproverId = Guid.NewGuid();
            invoiceApproval.ApproverEmail = "aylmer.carson@nowhere.com";
            invoiceApproval.ApprovedBy = "aylmer.carson";
            invoiceApproval.DateApproved = DateTime.UtcNow;
            invoiceApproval.Id = r.Id;

            return invoiceApproval;
        }
    }
}