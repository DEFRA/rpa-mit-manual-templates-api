using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class InvoiceLineForAzure
    {
        public decimal value { get; set; }

        public string accountCode { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string marketingYear { get; set; } = string.Empty;

        [JsonPropertyName("deliveryBody")]
        public string deliveryBodyCode { get; set; } = string.Empty;

        public string schemeCode { get; set; } = string.Empty;


        public string fundCode { get; set; } = string.Empty;
    }
}
