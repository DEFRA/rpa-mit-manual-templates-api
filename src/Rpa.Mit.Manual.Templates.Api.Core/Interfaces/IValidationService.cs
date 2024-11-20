using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IValidationService
    {
        Task<bool> FundCodeIsValid(IEnumerable<FundCode> fundCodes, string fundcode, string mainAccount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> CustomerIdIsValid(string customerId, string org, string schemeType, CancellationToken ct);

        Task<bool> MarketingYearIsValid(IEnumerable<MarketingYear> marketingYears, string year);


        string GetChartOfAccountDescription(
                                                IEnumerable<ChartOfAccounts> chartOfAccounts,
                                                IEnumerable<MainAccount> accountsAp,
                                                IEnumerable<SchemeCode> schemeCodes,
                                                IEnumerable<DeliveryBody> deliveryBodies,
                                                string mainAccount,
                                                string schemeCode,
                                                string deliveryBodyCode);
    }
}
