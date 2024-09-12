using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// a set of all the data uploaded in a single excel file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadArDataset
    {
        public BulkUploadInvoice? BulkUploadInvoice { get; set; }
        public List<BulkUploadApDetailLine> BulkUploadDetailLines { get; set; } = [];
    }
}
