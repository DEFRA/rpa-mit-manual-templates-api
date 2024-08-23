using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    /// <summary>
    /// camel casing to suit payment hub requirements
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestForAzure
    {

        [JsonIgnore]
        public string InvoiceRequestId { get; set; } = string.Empty;

        public int paymentRequestNumber { get; set; }

        public string invoiceNumber { get; set; } = string.Empty;

        public string agreementNumber { get; set; } = string.Empty;

        public string sourceSystem { get; set; } = "Manual";

        public string frn { get; set; } = string.Empty;

        public int sbi { get; set; } = 999999999;

        public string deliveryBody { get; set; } = string.Empty;

        public decimal value { get; set; }

        public string marketingYear { get; set; } = string.Empty;

        public string currency { get; set; } = "GBP";

        public IEnumerable<InvoiceLineForAzure> invoiceLines { get; set; } = Enumerable.Empty<InvoiceLineForAzure>();
    }
}
