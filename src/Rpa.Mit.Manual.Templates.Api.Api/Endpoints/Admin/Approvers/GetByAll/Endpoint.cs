using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace ApproversGetByAll
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : Endpoint<Request, Response>
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
            Get("admin/approvers/getbyall");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            Response response = new();

            try
            {
                response.AdminApprovers = await _iApproversAdminRepo.GetByAll(r.Email, r.DeliveryBody, r.SchemeCode, r.Threshold, ct);

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