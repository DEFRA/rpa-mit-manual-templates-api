using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    [ExcludeFromCodeCoverage]
    public sealed class PaymentHubJsonGenerator : IPaymentHubJsonGenerator
    {
        public string GenerateInvoiceRequestJson(InvoiceRequestForAzure invoiceRequest, CancellationToken ct)
        {
            return JsonSerializer.Serialize(invoiceRequest);
        }
    }
}
