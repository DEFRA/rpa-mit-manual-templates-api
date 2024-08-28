namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    public class PaymentHubResponseInvoiceLine
    {
        public int value { get; set; }
        public string accountCode { get; set; }
        public string description { get; set; }
        public string marketingYear { get; set; }
        public string deliveryBody { get; set; }
        public string schemeCode { get; set; }
        public string fundCode { get; set; }
        public bool convergence { get; set; }
        public bool stateAid { get; set; }
    }

    public class PaymentHubResponsePaymentRequest
    {
        public string InvoiceRequestId { get; set; }
        public int paymentRequestNumber { get; set; }
        public string invoiceNumber { get; set; }
        public string agreementNumber { get; set; }
        public string sourceSystem { get; set; }
        public string frn { get; set; }
        public int sbi { get; set; }
        public string deliveryBody { get; set; }
        public int value { get; set; }
        public string marketingYear { get; set; }
        public string currency { get; set; }
        public List<PaymentHubResponseInvoiceLine> invoiceLines { get; set; }
        public int schemeId { get; set; }
        public string correlationId { get; set; }
        public string ledger { get; set; }
        public string dueDate { get; set; }
    }

    public class PaymentHubResponseRoot
    {
        public PaymentHubResponsePaymentRequest paymentRequest { get; set; }
        public bool accepted { get; set; }
    }
}
