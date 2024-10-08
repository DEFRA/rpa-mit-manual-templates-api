using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetByInvoiceLineId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceLineByIdRequest
    {
        public Guid InvoiceLineId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceLineByIdResponse
    {
        public InvoiceLine? InvoiceLine { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
