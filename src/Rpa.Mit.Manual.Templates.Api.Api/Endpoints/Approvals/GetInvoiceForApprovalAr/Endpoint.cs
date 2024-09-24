using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetInvoiceForApprovalAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceForApprovalArEndpoint : Endpoint<GetInvoiceForApprovalArRequest, GetInvoiceForApprovalArResponse>
    {
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<GetInvoiceForApprovalArEndpoint> _logger;

        public GetInvoiceForApprovalArEndpoint(
            ILogger<GetInvoiceForApprovalArEndpoint> logger,
            IApprovalsRepo iApprovalsRepo)
        {
            _logger = logger;
            _iApprovalsRepo = iApprovalsRepo;
        }

        public override void Configure()
        {
            //TODO: need to restrict this ep by role/policy
            Post("/approvals/getinvoiceforapprovalar");
        }

        public override async Task HandleAsync(GetInvoiceForApprovalArRequest r, CancellationToken ct)
        {
            GetInvoiceForApprovalArResponse response = new();

            try
            {
                var approverEmail = User.Identity?.Name!;

                response.InvoiceAr = await _iApprovalsRepo.GetInvoiceArForApproval(r.InvoiceId, approverEmail, ct);

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