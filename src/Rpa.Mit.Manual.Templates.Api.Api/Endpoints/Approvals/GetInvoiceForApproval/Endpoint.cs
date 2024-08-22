using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetInvoiceForApproval
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalEndpoint : Endpoint<GetInvoiceForApprovalRequest, GetInvoiceForApprovalResponse>
    {
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<GetInvoiceForApprovalEndpoint> _logger;

        public GetInvoiceForApprovalEndpoint(
            ILogger<GetInvoiceForApprovalEndpoint> logger,
            IApprovalsRepo iApprovalsRepo)
        {
            _logger = logger;
            _iApprovalsRepo = iApprovalsRepo;
        }

        public override void Configure()
        {
            //TODO: need to restrict this ep by role/policy
            Post("/approvals/getinvoiceforapproval");
        }

        public override async Task HandleAsync(GetInvoiceForApprovalRequest r, CancellationToken ct)
        {

            GetInvoiceForApprovalResponse response = new();

            try
            {
                var approverEmail = User.Identity?.Name!;

                response.Invoice = await _iApprovalsRepo.GetInvoiceForApproval(r.InvoiceId, approverEmail, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}