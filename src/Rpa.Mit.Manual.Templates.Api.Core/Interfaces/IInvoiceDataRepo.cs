using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceDataRepo
    {
        Task<bool> AddInvoice(Invoice invoice);
    }
}
