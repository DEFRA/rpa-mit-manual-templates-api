using Microsoft.Extensions.Caching.Memory;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;

internal sealed class GetReferenceDataEndpoint : EndpointWithoutRequest<Response>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IReferenceDataRepo _iReferenceDataRepo;
    private readonly ILogger<GetReferenceDataEndpoint> _logger;

    public GetReferenceDataEndpoint(
                   ILogger<GetReferenceDataEndpoint> logger,
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
        Get("/referencedata/get");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        var response = new Response();
        var refData = new ReferenceData();

        try
        {
            if (!_memoryCache.TryGetValue(CacheKeys.ReferenceData, out refData))
            {
                refData = await _iReferenceDataRepo.GetAllReferenceData();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(1));

                _memoryCache.Set(CacheKeys.ReferenceData, refData, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.OrganisationReferenceData, refData.Organisations, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.PaymentTypesReferenceData, refData.PaymentTypes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.SchemeTypesReferenceData, refData.SchemeTypes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.SchemeCodesReferenceData, refData.SchemeCodes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.AccountCodesReferenceData, refData.AccountCodes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.DeliveryBodiesReferenceData, refData.DeliveryBodies, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.MarketingYearsReferenceData, refData.MarketingYears, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.FundCodesReferenceData, refData.FundCodes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.DeliveryBodiesInitialReferenceData, refData.FundCodes, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.SchemeInvoiceTemplateSecondaryQuestionsData, refData.SchemeInvoiceTemplateSecondaryQuestions, cacheEntryOptions);
            }

            response.ReferenceData = refData;

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