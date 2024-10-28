using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints;
using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint
{
    [ExcludeFromCodeCoverage]
    public class ReferenceDataRepo : BaseData, IReferenceDataRepo
    {
        private const int CacheDurationInDays = 60;
        private readonly IMemoryCache _memoryCache;
        private readonly ICacheManager _iCacheManager;

        public ReferenceDataRepo(
            IOptions<PostGres> options,
            ICacheManager iCacheManager,
             IMemoryCache memoryCache) : base(options)
        {
            _iCacheManager = iCacheManager;
            _memoryCache = memoryCache;
        }     

        public async Task<ReferenceData> GetAllReferenceData(CancellationToken ct)
        {
            var referenceData = new ReferenceData();

            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"
                        SELECT deliverybodydescription, code, accountcode, org FROM lookup_deliverybodyinitialselections;
                        SELECT name, code, deliverybodycode FROM lookup_schemeinvoicetemplates;
                        SELECT id, name FROM lookup_schemeinvoicetemplatessecondaryrpaquestions;
                        SELECT code, description FROM lookup_paymenttypes;
                        SELECT code, description FROM lookup_schemetypes;
                        SELECT code, description, org FROM lookup_schemecodes;
                        SELECT code, description FROM lookup_accountcodes;
                        SELECT code, description, org FROM lookup_deliverybodycodes;
                        SELECT code, description FROM lookup_marketingyearcodes;
                        SELECT code, description, org FROM lookup_fundcodes;
                        SELECT code, description, org FROM lookup_accounts_ap;
                        SELECT code, description, org FROM lookup_accounts_ar;

                        SELECT code, description, org FROM lookup_ap_chartofaccounts;
                        SELECT code, description, org FROM lookup_ar_chartofaccounts;
                        ";

                using (var res = await cn.QueryMultipleAsync(sql))
                {
                    referenceData.InitialDeliveryBodies = await res.ReadAsync<DeliveryBodyInitial>();
                    referenceData.SchemeInvoiceTemplates = await res.ReadAsync<SchemeInvoiceTemplate>();
                    referenceData.SchemeInvoiceTemplateSecondaryQuestions = await res.ReadAsync<SchemeInvoiceTemplateSecondaryQuestion>();
                    referenceData.PaymentTypes = await res.ReadAsync<PaymentType>();
                    referenceData.SchemeTypes = await res.ReadAsync<SchemeType>();
                    referenceData.SchemeCodes = await res.ReadAsync<SchemeCode>();
                    referenceData.AccountCodes = await res.ReadAsync<AccountCode>();
                    referenceData.DeliveryBodies = await res.ReadAsync<DeliveryBody>();
                    referenceData.MarketingYears = await res.ReadAsync<MarketingYear>();
                    referenceData.FundCodes = await res.ReadAsync<FundCode>();
                    referenceData.AccountAps = await res.ReadAsync<AccountAp>();
                    referenceData.AccountArs = await res.ReadAsync<AccountAr>();

                    referenceData.ChartOfAccountsAp = await res.ReadAsync<ChartOfAccounts>();
                    referenceData.ChartOfAccountsAr = await res.ReadAsync<ChartOfAccounts>();

                    return referenceData;
                }
            }
        }

        public async Task<IEnumerable<PaymentType>> GetCurrencyReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.CurrenciesReferenceData;

            IEnumerable<PaymentType> currencies;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code, description FROM lookup_paymenttypes;";

                    currencies = await cn.QueryAsync<PaymentType>(sql);

                    return currencies;
                }
            });


            //if (!_memoryCache.TryGetValue(CacheKeys.CurrenciesReferenceData, out currencies!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code, description FROM lookup_paymenttypes;";

            //        currencies = await cn.QueryAsync<PaymentType>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.CurrenciesReferenceData, currencies, cacheEntryOptions);
            //    }
            //}

            //return currencies;
        }

        public async Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData(CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"SELECT code, description FROM lookup_schemetypes;";

                return await cn.QueryAsync<SchemeType>(sql);
            }
        }

        public async Task<IEnumerable<ChartOfAccounts>> GetChartOfAccountsApReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.ApChartOfAccounts;

            IEnumerable<ChartOfAccounts> chartOfAccounts;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_ap_chartofaccounts;";

                    chartOfAccounts = await cn.QueryAsync<ChartOfAccounts>(sql);

                    return chartOfAccounts;
                }
            });

            //IEnumerable<ChartOfAccounts> chartOfAccounts;

            //if (!_memoryCache.TryGetValue(CacheKeys.ApChartOfAccounts, out chartOfAccounts!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code,description,org FROM lookup_ap_chartofaccounts;";

            //        chartOfAccounts = await cn.QueryAsync<ChartOfAccounts>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.ApChartOfAccounts, chartOfAccounts, cacheEntryOptions);
            //    }
            //}

            //return chartOfAccounts;
        }

        public async Task<IEnumerable<ChartOfAccounts>> GetChartOfAccountsArReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.ArChartOfAccounts;

            IEnumerable<ChartOfAccounts> chartOfAccounts;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_ar_chartofaccounts;";

                    chartOfAccounts = await cn.QueryAsync<ChartOfAccounts>(sql);

                    return chartOfAccounts;
                }
            });

            //IEnumerable<ChartOfAccounts> chartOfAccounts;

            //if (!_memoryCache.TryGetValue(CacheKeys.ArChartOfAccounts, out chartOfAccounts!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code,description,org FROM lookup_ar_chartofaccounts;";

            //        chartOfAccounts = await cn.QueryAsync<ChartOfAccounts>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.ArChartOfAccounts, chartOfAccounts, cacheEntryOptions);
            //    }
            //}

            //return chartOfAccounts;
        }

        public async Task<IEnumerable<AccountAr>> GetArMainAccountsReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.AccountsAr;

            IEnumerable<AccountAr> accountsAr;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org,type FROM lookup_accounts_ar;";

                    accountsAr = await cn.QueryAsync<AccountAr>(sql);

                    return accountsAr;
                }
            });

            //IEnumerable<AccountAr> accountsAr;

            //if (!_memoryCache.TryGetValue(CacheKeys.AccountsAr, out accountsAr!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code,description,org,type FROM lookup_accounts_ar;";

            //        accountsAr = await cn.QueryAsync<AccountAr>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.AccountsAr, accountsAr, cacheEntryOptions);
            //    }
            //}

            //return accountsAr;
        }

        public async Task<IEnumerable<FundCode>> GetFilteredFundcodes(string org, CancellationToken ct)
        {
            string key = CacheKeys.FundCodes;

            IEnumerable<FundCode> fundCodes;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_fundcodes;";

                    fundCodes = await cn.QueryAsync<FundCode>(sql);

                    return fundCodes.Where(x => x.Org.ToLower() == org.ToLower()).AsEnumerable();
                }
            });

            //IEnumerable<FundCode> fundCodes;

            //if (!_memoryCache.TryGetValue(CacheKeys.FundCodes, out fundCodes!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code,description,org FROM lookup_fundcodes;";

            //        fundCodes = await cn.QueryAsync<FundCode>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.FundCodes, fundCodes, cacheEntryOptions);
            //    }
            //}

            //return fundCodes.Where(x => x.Org.ToLower() == org.ToLower()).AsEnumerable();
        }

        public async Task<IEnumerable<AccountAp>> GetApMainAccountsReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.AccountsAp;

            IEnumerable<AccountAp> accountsAp;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_accounts_ap;";

                    accountsAp = await cn.QueryAsync<AccountAp>(sql);

                    return accountsAp;
                }
            });

            //IEnumerable<AccountAp> accountsAp;

            //if (!_memoryCache.TryGetValue(CacheKeys.AccountsAp, out accountsAp!))
            //{
            //    using (var cn = new NpgsqlConnection(await DbConn()))
            //    {
            //        if (cn.State != ConnectionState.Open)
            //            await cn.OpenAsync(ct);

            //        var sql = @"SELECT code,description,org FROM lookup_accounts_ap;";

            //        accountsAp = await cn.QueryAsync<AccountAp>(sql);

            //        var cacheEntryOptions = new MemoryCacheEntryOptions()
            //            .SetSlidingExpiration(TimeSpan.FromDays(CacheDurationInDays));

            //        _memoryCache.Set(CacheKeys.AccountsAp, accountsAp, cacheEntryOptions);
            //    }
            //}

            //return accountsAp;
        }

        public async Task<IEnumerable<SchemeType>> GetSchemeCodesReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.SchemeCodesReferenceData;

            IEnumerable<SchemeType> schemeCodes;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_schemecodes;";

                    schemeCodes = await cn.QueryAsync<SchemeType>(sql);

                    return schemeCodes;
                }
            });
        }

        public async Task<IEnumerable<DeliveryBody>> GetDeliveryBodiesReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.DeliveryBodiesReferenceData;

            IEnumerable<DeliveryBody> deliveryBodies;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_deliverybodycodes;";

                    deliveryBodies = await cn.QueryAsync<DeliveryBody>(sql);

                    return deliveryBodies;
                }
            });
        }
    }
}
