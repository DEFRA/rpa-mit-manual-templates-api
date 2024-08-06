using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class InvoiceLineForAzure
    {
        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        [JsonPropertyName("DeliveryBody")]
        public string DeliveryBodyCode { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;
    }
}
