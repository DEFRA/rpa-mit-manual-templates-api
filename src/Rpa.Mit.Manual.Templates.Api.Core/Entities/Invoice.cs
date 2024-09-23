using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// this represents an AP invoice
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class Invoice : InvoiceBase
    {
        public IEnumerable<InvoiceRequest> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();
    }
}
