using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceRequestsAr.GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public Guid InvoiceId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public IEnumerable<InvoiceRequestAr> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequestAr>();

        public string Message { get; set; } = string.Empty;
    }
}
