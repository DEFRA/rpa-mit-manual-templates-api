using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentHubJsonGenerator
    {
        string GenerateInvoiceRequestJson(InvoiceRequestForAzure invoiceRequest, CancellationToken ct);
    }
}
