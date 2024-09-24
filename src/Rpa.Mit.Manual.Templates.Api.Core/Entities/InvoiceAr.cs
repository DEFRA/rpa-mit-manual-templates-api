using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceAr : InvoiceBase
    {
        public IEnumerable<InvoiceRequestAr> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequestAr>();
    }
}
