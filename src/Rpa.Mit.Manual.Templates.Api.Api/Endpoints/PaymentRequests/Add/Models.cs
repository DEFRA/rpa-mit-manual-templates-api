using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace PaymentsRequests.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddPaymentRequest
    {
        public string PaymentRequestId { get; set; } = string.Empty;

        public Guid InvoiceId { get; set; }

        public string SourceSystem { get; set; } = "Manual";

        public string FRN { get; set; } = string.Empty;

        public string SBI { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public int PaymentRequestNumber { get; set; }

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
    }


    [ExcludeFromCodeCoverage]
    internal sealed class PaymentRequestResponse
    {
        public string Message { get; set; } = string.Empty;

        public PaymentRequest? PaymentRequest { get; set; }
    }
}
