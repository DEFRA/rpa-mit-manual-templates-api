
using System.Diagnostics.CodeAnalysis;

namespace InvoiceRequests.Update
{
    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceRequestRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        public string SourceSystem { get; set; } = "Manual";

        public string FRN { get; set; } = string.Empty;

        public string SBI { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public int InvoiceRequestNumber { get; set; }

        public string AgreementNumber { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public string OriginalInvoiceNumber { get; set; } = string.Empty;

        public DateTime OriginalSettlementDate { get; set; } = default!;

        public DateTime RecoveryDate { get; set; } = default!;

        public string InvoiceCorrectionReference { get; set; } = string.Empty;

        public string Vendor { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        internal sealed class Validator : Validator<UpdateInvoiceRequestRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class UpdateInvoiceRequestResponse
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
