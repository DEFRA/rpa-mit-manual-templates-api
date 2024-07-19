using System.Collections;
using System.Collections.Generic;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// a set of all the data uploaded in a single excel file
    /// </summary>
    public sealed class BulkUploadApDataset
    {
        public List<BulkUploadApHeaderLine> BulkuploadHeaderLines { get; set; } = new List<BulkUploadApHeaderLine>();
        public List<BulkUploadApDetailLine> BulkUploadDetailLines { get; set; } = new List<BulkUploadApDetailLine>();
    }
}
