using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : EndpointWithMapping<ApproveInvoiceRequest, ApproveInvoiceResponse, InvoiceApproval>
    {
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly IInvoiceRepo _iInvoiceDataRepo;
        private readonly IPaymentHubJsonGenerator _iPaymentHubJsonGenerator;
        private readonly IEventQueueService _iEventQueueService;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            ILogger<ApproveInvoiceEndpoint> logger, 
            IEventQueueService iEventQueueService,
            IApprovalsRepo iApprovalsRepo,
            IInvoiceRepo iInvoiceDataRepo,
            IPaymentHubJsonGenerator iPaymentHubJsonGenerator)
        {
            _logger = logger;
            _iEventQueueService = iEventQueueService;
            _iApprovalsRepo = iApprovalsRepo;
            _iInvoiceDataRepo = iInvoiceDataRepo;
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
                    //TODO: send to paymnent hub
                    var invoiceRequests = await _iApprovalsRepo.GetInvoiceRequestsForAzure(r.Id,  ct);

                    var ttt = await _iPaymentHubJsonGenerator.GenerateInvoiceRequestJson(invoiceRequests.First(), ct);


                    response.Result = true;
                    response.Message = "Invoice approved.";
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