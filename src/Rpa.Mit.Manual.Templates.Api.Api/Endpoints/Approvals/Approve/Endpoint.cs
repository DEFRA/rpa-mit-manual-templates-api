using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : EndpointWithMapping<ApproveInvoiceRequest, ApproveInvoiceResponse, InvoiceApproval>
    {
        private readonly IEmailService _iEmailService;
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            ILogger<ApproveInvoiceEndpoint> logger,
            IEmailService iEmailService,        
            IApprovalsRepo iApprovalsRepo)
        {
            _logger = logger;
            _iEmailService = iEmailService;
            _iApprovalsRepo = iApprovalsRepo;
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
                var ar = await _iEmailService.EmailApprovers(null, ct);

                InvoiceApproval approval = await MapToEntityAsync(r, ct);

                if (await _iApprovalsRepo.ApproveInvoice(approval, ct))
                {


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