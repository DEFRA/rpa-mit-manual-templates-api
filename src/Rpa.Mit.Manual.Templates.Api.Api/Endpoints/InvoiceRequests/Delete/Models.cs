using System.Diagnostics.CodeAnalysis;

namespace InvoiceRequests.Delete
{
    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceRequestRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        internal sealed class Validator : Validator<DeleteInvoiceRequestRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceRequestResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
