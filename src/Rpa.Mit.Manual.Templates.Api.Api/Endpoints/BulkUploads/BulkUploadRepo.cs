using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    public class BulkUploadRepo : BaseData, IBulkUploadRepo
    {
        public BulkUploadRepo(IOptions<ConnectionStrings> options) : base(options)
        { }
    }
}
