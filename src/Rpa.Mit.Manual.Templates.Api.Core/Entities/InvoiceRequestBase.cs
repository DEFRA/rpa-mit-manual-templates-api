using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestBase
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        public Guid InvoiceId { get; set; }

        public string Ledger { get; set; } = string.Empty;

        public string SourceSystem { get; set; } = "Manual";

        public string FRN { get; set; } = string.Empty;

        public int? SBI { get; set; } = null;

        public string MarketingYear { get; set; } = string.Empty;

        public int InvoiceRequestNumber { get; set; }

        public string AgreementNumber { get; set; } = string.Empty;

        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string AccountType { get; set; } = string.Empty;

        public string OriginalInvoiceNumber { get; set; } = string.Empty;

        public DateTime OriginalSettlementDate { get; set; } = default!;

        public DateTime RecoveryDate { get; set; } = default!;

        public string InvoiceCorrectionReference { get; set; } = string.Empty;

        public string Vendor { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;
    }
}
