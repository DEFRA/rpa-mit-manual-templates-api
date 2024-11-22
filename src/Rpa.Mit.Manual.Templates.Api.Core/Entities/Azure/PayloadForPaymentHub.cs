using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    /// <summary>
    /// AP invoice request for azure payment hub
    /// camel casing to suit payment hub requirements
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed record ApPayloadForPaymentHub
    {
        public string CreatorEmailAddress { get; set; } = string.Empty; 

        public IEnumerable<InvoiceRequestForAzure> InvoiceRequestsAp { get; set; } = Enumerable.Empty<InvoiceRequestForAzure>();
    }

    [ExcludeFromCodeCoverage]
    public sealed record ArPayloadForPaymentHub
    {
        public string CreatorEmailAddress { get; set; } = string.Empty;

        public IEnumerable<InvoiceRequestArForAzure> InvoiceRequestsAr { get; set; } = Enumerable.Empty<InvoiceRequestArForAzure>();
    }
}
