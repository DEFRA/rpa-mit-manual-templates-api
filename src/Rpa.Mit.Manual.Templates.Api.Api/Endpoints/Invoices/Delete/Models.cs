
using System.Diagnostics.CodeAnalysis;

namespace Invoices.Delete
{
    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
