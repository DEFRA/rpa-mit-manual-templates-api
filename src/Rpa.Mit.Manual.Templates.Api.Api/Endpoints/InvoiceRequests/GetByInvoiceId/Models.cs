using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceRequests.GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRequestsGetByInvoiceIdRequest
    {
        public Guid InvoiceId { get; set; }

        internal sealed class Validator : Validator<InvoiceRequestsGetByInvoiceIdRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRequestsGetByInvoiceIdResponse
    {
        public IEnumerable<InvoiceRequest> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();

        public string Message { get; set; } = string.Empty;
    }
}
