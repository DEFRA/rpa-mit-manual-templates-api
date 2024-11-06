using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IValidationService
    {
         Task<bool> ApBulkUploadIsValid(BulkUploadApDataset bulkUploadApDataset, string org, CancellationToken ct);

        Task<bool> FundCodeIsValid(string fundcode, string org, CancellationToken ct);

        Task<bool> ArBulkUploadIsValid(BulkUploadArDataset bulkUploadArDataset, string org, CancellationToken ct);


        string? GetChartOfAccountDescription(
                                                IEnumerable<ChartOfAccounts> chartOfAccounts,
                                                IEnumerable<AccountAr> accountsAp,
                                                IEnumerable<SchemeType> schemeTypes,
                                                IEnumerable<DeliveryBody> deliveryBodies,
                                                string mainAccount,
                                                string schemeCode,
                                                string deliveryBodyCode);
    }
}
