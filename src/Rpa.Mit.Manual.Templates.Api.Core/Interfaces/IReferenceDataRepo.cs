using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IReferenceDataRepo
    {
        /// <summary>
        /// gets all our lookup reference data and has it cached
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<ReferenceData> GetAllReferenceData(CancellationToken ct);

        public Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData(CancellationToken ct);

        /// <summary>
        /// loads the payment currency options. currently either EURO or GBP
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<PaymentType>> GetCurrencyReferenceData(CancellationToken ct);

        /// <summary>
        /// cached set of ap accounts data
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<ChartOfAccounts>> GetChartOfAccountsApReferenceData(CancellationToken ct);

        /// <summary>
        ///  cached set of chart ar accounts data
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<ChartOfAccounts>> GetChartOfAccountsArReferenceData(CancellationToken ct);

        /// <summary>
        /// cached set of lookup_accounts_ar data
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<AccountAr>> GetArMainAccountsReferenceData(CancellationToken ct);

        /// <summary>
        /// gets a list of filtered fundcodes for validation purposes, filtered on org
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<FundCode>> GetFilteredFundcodes(string org, CancellationToken ct);
    }
}
