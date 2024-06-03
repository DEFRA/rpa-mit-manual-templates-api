using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    public class InvoiceDataRepo : IInvoiceDataRepo
    {
        public async Task<bool> AddInvoice(Invoice invoice)
        {
            return true;
        }
    }
}
