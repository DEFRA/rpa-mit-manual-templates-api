namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    public sealed class SchemeInvoiceTemplate
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string DeliveryBodyCode { get; set; } = string.Empty;    
    }
}
