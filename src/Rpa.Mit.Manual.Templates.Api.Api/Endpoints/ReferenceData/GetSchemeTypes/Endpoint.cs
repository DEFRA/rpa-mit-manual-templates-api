using GetPaymentTypes;
using Microsoft.Extensions.Caching.Memory;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetSchemeTypes
{
    internal sealed class GetSchemeTypesEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetSchemeTypesEndpoint> _logger;

        public GetSchemeTypesEndpoint(
                       ILogger<GetSchemeTypesEndpoint> logger,
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
            Get("/schemetypes/get");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                if (!_memoryCache.TryGetValue(CacheKeys.OrganisationReferenceData, out IEnumerable<SchemeType> schemeTypes))
                {
                    schemeTypes = await _iReferenceDataRepo.GetSchemeTypeReferenceData(ct);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1));

                    _memoryCache.Set(CacheKeys.SchemeTypesReferenceData, schemeTypes, cacheEntryOptions);
                }

                response.SchemeTypes = schemeTypes;

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