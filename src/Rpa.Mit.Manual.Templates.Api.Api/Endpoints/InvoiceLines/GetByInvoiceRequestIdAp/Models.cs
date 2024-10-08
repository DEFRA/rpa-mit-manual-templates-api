using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceLines.GetByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLinesGetByInvoiceRequestIdRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLinesGetByInvoiceRequestIdResponse
    {
        public IEnumerable<InvoiceLine> InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLine>();

        public string Message { get; set; } = string.Empty;
    }
}
