using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// represents an AP invoice request
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRequest : InvoiceRequestBase
    {
        public IEnumerable<InvoiceLine>? InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLine>();
    }
}
