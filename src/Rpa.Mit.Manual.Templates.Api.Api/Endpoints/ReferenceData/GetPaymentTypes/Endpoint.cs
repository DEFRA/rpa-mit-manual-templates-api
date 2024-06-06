using Microsoft.Extensions.Caching.Memory;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetPaymentTypes
{
    internal sealed class GetPaymentTypesEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetPaymentTypesEndpoint> _logger;

        public GetPaymentTypesEndpoint(
                       ILogger<GetPaymentTypesEndpoint> logger,
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
            Get("/paymenttypes/get");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                IEnumerable<PaymentType>? paymentTypes;

                if (!_memoryCache.TryGetValue(CacheKeys.OrganisationReferenceData, out paymentTypes))
                {
                    paymentTypes = await _iReferenceDataRepo.GetPaymentTypeReferenceData(ct);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1));

                    _memoryCache.Set(CacheKeys.PaymentTypesReferenceData, paymentTypes, cacheEntryOptions);
                }

                response.PaymentTypes = paymentTypes!;

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}