using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// a set of all the data uploaded in a single excel file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadApDataset
    {
        public Invoice? Invoice { get; set; }
        public List<BulkUploadApHeaderLine> BulkuploadHeaderLines { get; set; } = [];
        public List<BulkUploadApDetailLine> BulkUploadDetailLines { get; set; } = [];
    }
}
