namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// a set of all the data uploaded in a single excel file
    /// </summary>
    public sealed class BulkUploadApDataset
    {
        public IEnumerable<BulkUploadApHeaderLine> BulkuploadHeaderLines { get; set; } = Enumerable.Empty<BulkUploadApHeaderLine>();
        public IEnumerable<BulkUploadApDetailLine> BulkUploadDetailLines { get; set; } = Enumerable.Empty<BulkUploadApDetailLine>();
    }
}
