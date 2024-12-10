namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface INamedRangeService
    {
        string GetFundCodeNamedRange(string org, string dBody, string invoiceType);

        string GetAccountNamedRange(string org, string dBody, string invoiceType);

        string GetSchemeTypeNamedRange(string org, string dBody, string invoiceType);

        string GetMarketingYearNamedRange(string org, string dBody);

        string GetDeliveryBodyNamedRange(string org, string dBody);
    }
}
