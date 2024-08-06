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
            // temp allow anon
            AllowAnonymous();
            Post("/approvals/getinvoiceforapproval");
        }

        public override async Task HandleAsync(GetInvoiceForApprovalRequest r, CancellationToken ct)
        {

            GetInvoiceForApprovalResponse response = new();

            try
            {
                //TODO: this needs to be changed to trap the logged-in authenticated user
                var approverEmail = "ding.dong@us.com";

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