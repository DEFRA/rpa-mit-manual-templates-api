using System.Diagnostics.CodeAnalysis;

namespace Invoices.ToAzure
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceToAzureRequest
    {

        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<InvoiceToAzureRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceToAzureResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
