namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface INamedRangeService
    {
        /// <summary>
        /// calculate the named range for fund codes
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        string GetFundCodeNamedRange(string org, string dBody, string invoiceType);

        /// <summary>
        /// calculate the named range for main accounts
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        string GetAccountNamedRange(string org, string dBody, string invoiceType);

        /// <summary>
        /// calculate the named range for scheme types
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        string GetSchemeTypeNamedRange(string org, string dBody, string invoiceType);

        /// <summary>
        /// calculate the named range for marketing years
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <returns></returns>
        string GetMarketingYearNamedRange(string org, string dBody, string invoiceType);

        /// <summary>
        /// calculate the named range for delivery bodies
        /// </summary>
        /// <param name="org"></param>
        /// <param name="dBody"></param>
        /// <returns></returns>
        string GetDeliveryBodyNamedRange(string org, string dBody, string invoiceType);
    }
}
