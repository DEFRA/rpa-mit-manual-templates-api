using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Dropdowns.GetAll
{
    internal sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        private readonly IDropdownRepo _iDropdownRepo;
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(
            ILogger<Endpoint> logger,
            IDropdownRepo iDropdownRepo)
        {
            _logger = logger;
            _iDropdownRepo = iDropdownRepo;
        }

        public override void Configure()
        {
            Get("dropdowns/getall");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.Dropdowns = await _iDropdownRepo.GetAll(ct);

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