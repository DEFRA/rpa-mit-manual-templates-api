using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// a set of all the data uploaded in a single excel file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadArDataset
    {
        /// <summary>
        /// total number of invoice detail lines uploaded
        /// </summary>
        public int NumberOfInvoices { get; set; }

        /// <summary>
        /// total sum of invoices uploaded
        /// </summary>
        public decimal InvoiceTotal { get; set; }

        public BulkUploadInvoice? BulkUploadInvoice { get; set; }
        public List<BulkUploadArDetailLine> BulkUploadDetailLines { get; set; } = [];
    }
}
