using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceLine
    {
        public Guid Id { get; set; }

        public string PaymentRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBody { get; set; } = string.Empty;
    }
}
