using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices.InvoiceToAzure
{
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

    internal sealed class InvoiceToAzureResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
