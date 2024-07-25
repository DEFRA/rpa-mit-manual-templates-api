using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceRequestIdRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        internal sealed class Validator : Validator<GetByInvoiceRequestIdRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceRequestIdResponse
    {
        public InvoiceRequest? InvoiceRequest { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
