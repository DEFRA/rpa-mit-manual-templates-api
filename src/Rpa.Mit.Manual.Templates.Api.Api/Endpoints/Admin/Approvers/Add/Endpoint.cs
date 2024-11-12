using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace AdminAdd
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : EndpointWithMapping<Request, Response, AdminApprover>
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
            Post("admin/approvers/add");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            Response response = new();

            try
            {
                AdminApprover adminApprover = await MapToEntityAsync(r, ct);

                response.Result = await _iApproversAdminRepo.Create(adminApprover, ct);

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public override async Task<AdminApprover> MapToEntityAsync(Request r, CancellationToken ct = default)
        {
            var adminApprover = await Task.FromResult(new AdminApprover());

            adminApprover.Email = r.Email;
            adminApprover.SchemeCode = r.SchemeCode;
            adminApprover.DeliveryBody = r.DeliveryBody;
            adminApprover.Threshold = r.Threshold;

            return adminApprover;
        }
    }
}