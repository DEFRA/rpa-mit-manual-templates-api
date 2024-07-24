using System.Data;
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    [ExcludeFromCodeCoverage]
    public interface IApImporterService
    {
        Task<BulkUploadApDataset> ImportAPData(DataTable data, CancellationToken ct);
    }
}
