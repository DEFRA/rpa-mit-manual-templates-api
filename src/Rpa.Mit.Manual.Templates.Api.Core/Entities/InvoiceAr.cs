namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    public sealed class InvoiceAr : InvoiceBase
    {
        public IEnumerable<InvoiceRequestAr> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequestAr>();
    }
}
