using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IImporterService
    {
        Task<BulkUploadImportResult<BulkUploadApDataset, string>> ImportAPData(DataTable data, string org, string schemeInvoiceTemplate, CancellationToken ct);
    }

    public interface IArImporterService
    {
        Task<BulkUploadImportResult<BulkUploadArDataset, string>> ImportARData(DataTable data, string org, string schemeInvoiceTemplate, CancellationToken ct);
    }
}
