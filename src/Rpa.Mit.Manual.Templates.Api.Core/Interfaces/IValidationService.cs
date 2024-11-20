using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IValidationService
    {
        Task<bool> FundCodeIsValid(IEnumerable<FundCode> fundCodes, string fundcode, string mainAccount);

        Task<bool> MainAccountIsValid(IEnumerable<MainAccount> mainAccounts, string mainAccount, string org);

        /// <summary>
        /// requires a length of 20
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> InvoiceRequestIdHasCorrectLength(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// Test Invoice Request amount - is within range set by Finance < Abs(1 billion) (this is the total for each Invoice Request)
        /// </summary>
        /// <param name="invoiceRequestAmount"></param>
        /// <returns></returns>
        Task<bool> InvoiceRequestAmountIsOk(decimal invoiceRequestAmount);

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
