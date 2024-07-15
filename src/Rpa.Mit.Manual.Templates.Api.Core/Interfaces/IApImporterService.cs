using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApImporterService
    {
        Task<BulkUploadApDataset> ImportAPData(DataTable data, CancellationToken ct);
    }
}
