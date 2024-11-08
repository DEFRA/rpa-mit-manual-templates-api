using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApImporterService
    {
        Task<BulkUploadImportResult<BulkUploadApDataset, string>> ImportAPData(DataTable data, string org, string schemeInvoiceTemplate, CancellationToken ct);
    }

    public interface IArImporterService
    {
        Task<BulkUploadArDataset> ImportARData(DataTable data, string org, CancellationToken ct);
    }
}
