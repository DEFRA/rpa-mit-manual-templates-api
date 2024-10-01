namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentHubJsonGenerator
    {
        string GenerateInvoiceRequestJson<T>(T invoiceRequest, CancellationToken ct) where T:class;
    }
}
