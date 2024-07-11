using System.Diagnostics.CodeAnalysis;

namespace DeleteInvoiceLine
{
    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceLineRequest
    {
        public Guid InvoiceLineId { get; set; }

        internal sealed class Validator : Validator<DeleteInvoiceLineRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public decimal InvoiceRequestValue { get; set; }
    }
}
