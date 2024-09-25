using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    /// <summary>
    /// AP invoice request for azure payment hub
    /// camel casing to suit payment hub requirements
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record InvoiceRequestForAzure : InvoiceRequestForAzureBase
    {
        public IEnumerable<InvoiceLineForAzure> invoiceLines { get; set; } = Enumerable.Empty<InvoiceLineForAzure>();
    }
}
