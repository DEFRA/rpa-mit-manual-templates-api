using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IValidationService
    {
        Task<bool> ApBulkUploadIsValid(BulkUploadApDataset bulkUploadApDataset, string deliveryBody, CancellationToken ct);
    }
}
