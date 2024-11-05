using System.Diagnostics.CodeAnalysis;

using GetMyApprovals;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Approvers
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        private readonly IApproversAdminRepo _iApproversAdminRepo;
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(
            ILogger<Endpoint> logger,
            IApproversAdminRepo iApproversAdminRepo)
        {
            _logger = logger;
            _iApproversAdminRepo = iApproversAdminRepo;
        }

        public override void Configure()
        {
            Get("admin/approvers/getall");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Response response = new();

            try
            {
                response.AdminApprovers = await _iApproversAdminRepo.FindAlllookup_approvers();

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