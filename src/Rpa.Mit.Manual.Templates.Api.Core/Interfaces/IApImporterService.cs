using System.Data;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApImporterService
    {
        Task<bool> ImportAPData(DataTable data, CancellationToken ct);
    }
}
