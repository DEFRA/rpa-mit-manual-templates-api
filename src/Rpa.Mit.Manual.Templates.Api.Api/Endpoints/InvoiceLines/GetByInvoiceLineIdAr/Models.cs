using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetByInvoiceLineIdAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceLineByIdArRequest
    {
        public Guid InvoiceLineId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class GetInvoiceLineByIdArResponse
    {
        public InvoiceLineAr? InvoiceLine { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
