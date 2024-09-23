namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    public class InvoiceAr : InvoiceBase
    {
        public IEnumerable<InvoiceRequestAr> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequestAr>();
    }
}
