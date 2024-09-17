using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace RejectInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class RejectInvoiceEndpoint : EndpointWithMapping<RejectInvoiceRequest, RejectInvoiceResponse, InvoiceRejection>
    {
        private readonly IEmailService _iEmailService;
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<RejectInvoiceEndpoint> _logger;

        public RejectInvoiceEndpoint(
            ILogger<RejectInvoiceEndpoint> logger,
            IEmailService iEmailService,    
            IApprovalsRepo iApprovalsRepo)
        {
            _logger = logger;
            _iEmailService = iEmailService;
            _iApprovalsRepo = iApprovalsRepo;
        }

        public override void Configure()
        {
            Post("/approvals/reject");
        }

        public override async Task HandleAsync(RejectInvoiceRequest r, CancellationToken ct)
        {
            RejectInvoiceResponse response = new();
            response.Result = false;

            try
            {
                InvoiceRejection rejection = await MapToEntityAsync(r, ct);

                if (await _iApprovalsRepo.RejectInvoice(rejection, ct))
                {
                    response.Result = await _iEmailService.EmailInvoiceRejection(rejection.ApproverEmail, r.InvoiceId, ct);

                    response.Message = "Invoice rejected.";
                }
                else
                {
                    response.Message = "Error rejecting invoice.";
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

        public sealed override async Task<InvoiceRejection> MapToEntityAsync(RejectInvoiceRequest r, CancellationToken ct = default)
        {
            var invoiceRejection = await Task.FromResult(new InvoiceRejection());

            invoiceRejection.ApproverEmail = User.Identity?.Name!;
            invoiceRejection.DateApproved = DateTime.UtcNow;
            invoiceRejection.Reason = r.Reason;
            invoiceRejection.Id = r.InvoiceId;

            return invoiceRejection;
        }
    }
}