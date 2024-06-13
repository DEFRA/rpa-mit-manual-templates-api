
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceLineRequest
    {
        public Guid Id { get; set; }

        public string PaymentRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; }  = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBody { get; set; } = string.Empty;

    }

    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLine? InvoiceLine { get; set; }
    }
}
