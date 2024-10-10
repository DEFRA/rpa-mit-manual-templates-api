using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BulkUploadApHeaderLine
    {
        public Guid InvoiceId { get; set; }

        public string InvoiceRequestId { get; set; } = string.Empty;

        /// <summary>
        /// this is the account type (AP or AR)
        /// </summary>
        public string Ledger { get; set; } = string.Empty;


        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        public string Frn { get; set; } = string.Empty;

        public int? Sbi { get; set; } = null;

        public string Vendor { get; set; } = string.Empty;

        public string AgreementNumber { get; set; } = string.Empty;

        public string PaymentType { get; set; } = string.Empty;

        public string DueDate { get; set; } = string.Empty;

        /// <summary>
        /// Calulated field
        /// </summary>
        public decimal TotalAmount { get; set; } = 0.0M;

        public string Description { get; set; } = string.Empty;

        public List<BulkUploadApDetailLine>? BulkUploadApDetailLines { get; set; }
    }
}
