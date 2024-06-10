using Microsoft.Extensions.Caching.Memory;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetOrganisations
{
    internal sealed class GetOrganisationsEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetOrganisationsEndpoint> _logger;

        public GetOrganisationsEndpoint(
                       ILogger<GetOrganisationsEndpoint> logger,
                       IMemoryCache memoryCache,
                       IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/organisations/get");
            ResponseCache(600); //cache seconds
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.Organisations = await _iReferenceDataRepo.GetOrganisationsReferenceData(ct);

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