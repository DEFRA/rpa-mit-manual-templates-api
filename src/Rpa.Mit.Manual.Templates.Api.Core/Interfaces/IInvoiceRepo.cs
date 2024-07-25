using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceRepo
    {
        /// <summary>
        /// adds an invoice 'header' to the db
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddInvoice(Invoice invoice, CancellationToken ct);


        /// <summary>
        /// get an entire invoice, with all children, for publishing to azure servicebus
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Invoice> GetInvoiceForAzure(Guid invoiceId, CancellationToken ct);

        Task<Invoice> GetInvoiceByInvoiceId(Guid invoiceId, CancellationToken ct);

        Task<IEnumerable<Invoice>> GetAllInvoices( CancellationToken ct);

        /// <summary>
        /// deletes an invoice and all its children
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteInvoice(Guid invoiceId, CancellationToken ct);
    }
}
