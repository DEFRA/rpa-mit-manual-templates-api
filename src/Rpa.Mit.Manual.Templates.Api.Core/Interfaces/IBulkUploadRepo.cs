using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IBulkUploadRepo
    {
        Task<bool> AddApBulkUpload(BulkUploadApDataset bulkUploadApDataset, CancellationToken ct);

        Task<string> GetDetailLineDescripions(string query, CancellationToken ct);
    }
}
