using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceLineRepo
    {
        /// <summary>
        /// adds an invoice line to the db
        /// </summary>
        /// <param name="invoiceLine"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct);
    }
}
