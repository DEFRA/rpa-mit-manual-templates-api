using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace UpdateInvoiceLine
{
    internal sealed class UpdateInvoiceLineRequest
    {

        public Guid Id { get; set; }

        public string InvoiceRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBody { get; set; } = string.Empty;

        internal sealed class Validator : Validator<UpdateInvoiceLineRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class UpdateInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLine? InvoiceLine { get; set; }

        public decimal InvoiceRequestValue { get; set; }
    }
}
