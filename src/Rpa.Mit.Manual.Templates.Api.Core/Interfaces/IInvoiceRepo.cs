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
        /// get single invoice by its id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Invoice> GetInvoiceByInvoiceId(Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// gets all invoices
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<Invoice>> GetAllInvoices( CancellationToken ct);

        /// <summary>
        /// deletes an invoice and all its children
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteInvoice(Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// gets the email address of the creator of the invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<string> GetInvoiceCreatorEmail(Guid invoiceId, CancellationToken ct);
    }
}
