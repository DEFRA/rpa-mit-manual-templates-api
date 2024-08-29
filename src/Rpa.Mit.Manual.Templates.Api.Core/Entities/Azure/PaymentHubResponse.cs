﻿namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    public class PaymentHubResponseInvoiceLine
    {
        public decimal value { get; set; }
        public string accountCode { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string marketingYear { get; set; } = string.Empty;
        public string deliveryBody { get; set; } = string.Empty;
        public string schemeCode { get; set; } = string.Empty;
        public string fundCode { get; set; } = string.Empty;
        public bool convergence { get; set; }
        public bool stateAid { get; set; }
    }

    public class PaymentHubResponsePaymentRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;
        public int paymentRequestNumber { get; set; }
        public string invoiceNumber { get; set; } = string.Empty;
        public string agreementNumber { get; set; } = string.Empty;
        public string sourceSystem { get; set; } = string.Empty;
        public string frn { get; set; } = string.Empty;
        public int sbi { get; set; }
        public string deliveryBody { get; set; } = string.Empty;
        public int value { get; set; }
        public string marketingYear { get; set; } = string.Empty;
        public string currency { get; set; } = string.Empty;
        public List<PaymentHubResponseInvoiceLine>? invoiceLines { get; set; }
        public int schemeId { get; set; }
        public string correlationId { get; set; } = string.Empty;
        public string ledger { get; set; } = string.Empty;
        public string dueDate { get; set; } = string.Empty;
    }

    public class PaymentHubResponseRoot
    {
        public PaymentHubResponsePaymentRequest? paymentRequest { get; set; }
        public bool accepted { get; set; }

        public string error { get; set; } = string.Empty;
    }

    public class PaymentHubResponseForDatabase
    {
        public string invoicerequestid { get; set; } = string.Empty;

        public DateTime? paymenthubdateprocessed { get; set; }

        public bool accepted { get; set; }

        public string error { get; set; } = string.Empty;
    }
}