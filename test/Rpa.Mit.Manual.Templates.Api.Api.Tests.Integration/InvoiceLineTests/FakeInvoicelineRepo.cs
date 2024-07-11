using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests
{
    internal class FakeInvoicelineRepo : IInvoiceLineRepo
    {
        public Task<decimal> AddInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct) 
            => Task.FromResult(invoiceLine.Value);

        public Task<decimal> DeleteInvoiceLine(Guid invoiceLineId, CancellationToken ct)
            => Task.FromResult(34.55M);

        public Task<decimal> UpdateInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct)
            => Task.FromResult(34.55M);
    }
}
