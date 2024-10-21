using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceRequestsAp.GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRequestsGetByInvoiceIdRequest
    {
        public Guid InvoiceId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRequestsGetByInvoiceIdResponse
    {
        public IEnumerable<InvoiceRequest> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();

        public string Message { get; set; } = string.Empty;
    }
}
