using System.Diagnostics.CodeAnalysis;

namespace RejectInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class RejectInvoiceRequest
    {

        public Guid Id { get; set; }

        public string Reason { get; set; } = string.Empty;

        internal sealed class Validator : Validator<RejectInvoiceRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class RejectInvoiceResponse
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
