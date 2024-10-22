using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetArByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class Request
    {
        public string InvoiceRequestId { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetArByInvoiceRequestIdResponse
    {
        public InvoiceRequestAr? InvoiceRequest { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
