using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetMyApprovals
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetMyApprovalsEndpoint : EndpointWithoutRequest<GetMyApprovalsResponse>
    {
        private readonly IApprovalsRepo _iApprovalsRepo;
        private readonly ILogger<GetMyApprovalsEndpoint> _logger;

        public GetMyApprovalsEndpoint(
            ILogger<GetMyApprovalsEndpoint> logger,
            IApprovalsRepo iApprovalsRepo)
        {
            _logger = logger;
            _iApprovalsRepo = iApprovalsRepo;
        }

        public override void Configure()
        {
            //TODO: need to restrict this ep by role/policy
            Get("/approvals/getmyapprovals");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            GetMyApprovalsResponse response = new();

            try
            {
                var approverEmail = User.Identity?.Name!;

                response.Invoices = await _iApprovalsRepo.GetMyApprovals(approverEmail, ct);

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