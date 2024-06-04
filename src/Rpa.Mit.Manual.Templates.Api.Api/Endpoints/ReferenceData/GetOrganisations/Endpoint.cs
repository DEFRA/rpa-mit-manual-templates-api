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
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var response = new Response();

            try
            {
                if (!_memoryCache.TryGetValue(CacheKeys.OrganisationReferenceData, out IEnumerable<Organisation> organisations))
                {
                    organisations = await _iReferenceDataRepo.GetOrganisationsReferenceData();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1));

                    _memoryCache.Set(CacheKeys.OrganisationReferenceData, organisations, cacheEntryOptions);
                }

                response.Organisations = organisations;

                await SendAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Message = ex.Message;

                await SendAsync(response);
            }
        }
    }
}