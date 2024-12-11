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
        /// deletes an invoice and all its children.
        /// the database has cascading deletes configured for this so only need to delete the parent
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
        Task<string> GetInvoiceCreatorEmailAddress(Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// gets a list of the various dropdown values once a user has made their initial selections
        /// </summary>
        /// <param name="dropdownsRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DropdownsResponse> GetDropdowns(DropdownsRequest dropdownsRequest, CancellationToken ct);
    }
}
