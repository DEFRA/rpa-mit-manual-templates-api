using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentHubJsonGenerator
    {
        Task<string> GenerateInvoiceRequestJson(InvoiceRequestForAzure invoiceRequest, CancellationToken ct);
    }
}
