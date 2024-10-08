using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace InvoiceLinesArByInvoiceRequestIdEndpoint
{
    namespace InvoiceLinesAr.GetByInvoiceRequestId
    {
        [ExcludeFromCodeCoverage]
        internal sealed class InvoiceLinesArGetByInvoiceRequestIdRequest
        {
            public string InvoiceRequestId { get; set; } = string.Empty;
        }

        [ExcludeFromCodeCoverage]
        internal sealed class InvoiceLinesArGetByInvoiceRequestIdResponse
        {
            public IEnumerable<InvoiceLineAr> InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLineAr>();

            public string Message { get; set; } = string.Empty;
        }
    }
}