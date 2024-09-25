namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    public record InvoiceRequestForAzureBase
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        /// <summary>
        /// this is the account code, either "AP" or "AR"
        /// </summary>
        public string leger { get; set; } = string.Empty;

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
    }
}
