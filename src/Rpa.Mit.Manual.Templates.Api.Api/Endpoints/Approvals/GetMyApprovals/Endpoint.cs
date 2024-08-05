using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetMyApprovals
{
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
            // temp allow anon
            AllowAnonymous();
            Post("/approvals/getmyapprovals");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            GetMyApprovalsResponse response = new();

            try
            {
                //TODO: this needs to be changed to trap the logged-in authenticated user
                var approverEmail = "ding.dong@us.com";

                response.Invoices = await _iApprovalsRepo.GetMyApprovals(approverEmail, ct);

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