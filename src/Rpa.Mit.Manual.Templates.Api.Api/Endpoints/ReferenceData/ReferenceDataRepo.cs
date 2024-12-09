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
        private readonly ICacheManager _iCacheManager;


        public ReferenceDataRepo(
            IOptions<PostGres> options,
            ICacheManager iCacheManager) : base(options)
        {
            _iCacheManager = iCacheManager;
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
                        SELECT name, code, deliverybodycode, id FROM lookup_schemetypes;
                        SELECT id, name FROM lookup_schemeinvoicetemplatessecondaryrpaquestions;
                        SELECT code, description FROM lookup_paymenttypes;
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
                    referenceData.SchemeTypes = await res.ReadAsync<SchemeType>();
                    referenceData.SchemeInvoiceTemplateSecondaryQuestions = await res.ReadAsync<SchemeInvoiceTemplateSecondaryQuestion>();
                    referenceData.PaymentTypes = await res.ReadAsync<PaymentType>();
                    referenceData.SchemeCodes = await res.ReadAsync<SchemeCode>();
                    referenceData.AccountCodes = await res.ReadAsync<AccountCode>();
                    referenceData.DeliveryBodies = await res.ReadAsync<DeliveryBody>();
                    referenceData.MarketingYears = await res.ReadAsync<MarketingYear>();
                    referenceData.FundCodes = await res.ReadAsync<FundCode>();
                    referenceData.AccountAps = await res.ReadAsync<AccountAp>();
                    referenceData.AccountArs = await res.ReadAsync<MainAccount>();

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
        }

        public async Task<IEnumerable<MainAccount>> GetApMainAccountsReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.AccountsAp;

            IEnumerable<MainAccount> accountsAp;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_accounts_ap;";

                    accountsAp = await cn.QueryAsync<MainAccount>(sql);

                    return accountsAp;
                }
            });
        }

        public async Task<IEnumerable<MainAccount>> GetArMainAccountsReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.AccountsAr;

            IEnumerable<MainAccount> accountsAr;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org,type FROM lookup_accounts_ar;";

                    accountsAr = await cn.QueryAsync<MainAccount>(sql);

                    return accountsAr;
                }
            });
        }

        public async Task<IEnumerable<FundCode>> GetFundcodes(CancellationToken ct)
        {
            string key = CacheKeys.FundCodes;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_fundcodes;";

                    return await cn.QueryAsync<FundCode>(sql);
                }
            });
        }

        public async Task<IEnumerable<MarketingYear>> GetMarketingYears(CancellationToken ct)
        {
            string key = CacheKeys.MarketingYears;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code, description FROM lookup_marketingyearcodes;";

                    return await cn.QueryAsync<MarketingYear>(sql);
                }
            });
        }

        public async Task<IEnumerable<FundCode>> GetFilteredFundcodes(string org, CancellationToken ct)
        {
            string key = CacheKeys.FundCodesFiltered;

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
        }

        public async Task<IEnumerable<SchemeCode>> GetSchemeCodesReferenceData(CancellationToken ct)
        {
            string key = CacheKeys.SchemeCodesReferenceData;

            IEnumerable<SchemeCode> schemeCodes;

            return await _iCacheManager.Get(key, async () =>
            {
                using (var cn = new NpgsqlConnection(await DbConn()))
                {
                    if (cn.State != ConnectionState.Open)
                        await cn.OpenAsync(ct);

                    var sql = @"SELECT code,description,org FROM lookup_schemecodes;";

                    schemeCodes = await cn.QueryAsync<SchemeCode>(sql);

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
