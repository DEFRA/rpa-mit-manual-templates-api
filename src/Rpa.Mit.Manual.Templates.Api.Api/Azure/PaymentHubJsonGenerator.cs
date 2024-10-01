using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    [ExcludeFromCodeCoverage]
    public sealed class PaymentHubJsonGenerator : IPaymentHubJsonGenerator
    {
        public string GenerateInvoiceRequestJson<T>(T invoiceRequest, CancellationToken ct) where T : class => JsonSerializer.Serialize(invoiceRequest);
    }
}
