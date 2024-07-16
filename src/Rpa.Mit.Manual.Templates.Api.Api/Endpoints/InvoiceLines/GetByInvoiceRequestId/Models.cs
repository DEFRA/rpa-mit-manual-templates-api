using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceLines.GetByInvoiceRequestId
{
    internal sealed class InvoiceLinesGetByInvoiceRequestIdRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        internal sealed class Validator : Validator<InvoiceLinesGetByInvoiceRequestIdRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class InvoiceLinesGetByInvoiceRequestIdResponse
    {
        public IEnumerable<InvoiceLine> InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLine>();

        public string Message { get; set; } = string.Empty;
    }
}
