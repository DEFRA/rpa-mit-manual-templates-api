using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceRequestTests
{
    internal class FakeInvoiceRequestRepo : IInvoiceRequestRepo
    {
        public Task<bool> AddInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct)
            => Task.FromResult(true);

        public Task<bool> DeleteInvoiceRequest(string invoiceRequestId, CancellationToken ct)
            => Task.FromResult(true);

        public Task<decimal> GetInvoiceRequestValue(string invoiceRequestId, CancellationToken ct)
            => Task.FromResult(55.55M);

        public Task<bool> UpdateInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct)
            => Task.FromResult(true);

        public Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsByInvoiceId(Guid invoiceId, CancellationToken ct)
            => Task.FromResult(Enumerable.Empty<InvoiceRequest>());

        Task<InvoiceRequest> IInvoiceRequestRepo.GetInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct)
            => Task.FromResult(new InvoiceRequest());

        Task<bool> IInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(PaymentHubResponseForDatabase paymentHubResponseForDatabase)
            => Task.FromResult(true);
    }
}
