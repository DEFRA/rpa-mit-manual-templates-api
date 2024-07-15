using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    public class ApImporterService : IApImporterService
    {
        public Task<bool> ImportAPData(DataTable data, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
