using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestForAzure
    {
        public int paymentRequestNumber { get; set; }


        [JsonIgnore]
        public string InvoiceRequestId { get; set; } = string.Empty;

        public string invoiceNumber { get; set; } = string.Empty;

        public string agreementNumber { get; set; } = string.Empty;

        public string sourceSystem { get; set; } = "Manual";

        public string frn { get; set; } = string.Empty;

        public int sbi { get; set; } = 123456789;

        public string deliveryBody { get; set; } = string.Empty;

        public decimal value { get; set; }

        public string marketingYear { get; set; } = string.Empty;

        public string currency { get; set; } = "GBP";

        //public string DueDate { get; set; } = string.Empty;


        //public string AccountType { get; set; } = string.Empty;

        //public string Vendor { get; set; } = string.Empty;

        public IEnumerable<InvoiceLineForAzure> invoiceLines { get; set; } = Enumerable.Empty<InvoiceLineForAzure>();
    }
}
