using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace DeleteInvoice
{
    internal sealed class DeleteInvoiceRequest
    {
        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<DeleteInvoiceRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class DeleteInvoiceResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
