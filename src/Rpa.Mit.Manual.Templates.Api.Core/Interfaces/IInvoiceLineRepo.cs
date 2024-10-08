using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceLineRepo
    {
        /// <summary>
        /// adds an AP invoice line to the db and returns the new total value for the parent invoice request
        /// </summary>
        /// <param name="invoiceLine"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> AddInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct);

        /// <summary>
        /// adds an AR invoice line to the db and returns the new total value for the parent invoice request
        /// </summary>
        /// <param name="invoiceLine"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> AddInvoiceLineAr(InvoiceLineAr invoiceLineAr, CancellationToken ct);

        /// <summary>
        /// delete a single invoice line and return the new total value for the parent invoice request
        /// </summary>
        /// <param name="invoiceLineId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> DeleteInvoiceLine(Guid invoiceLineId, CancellationToken ct);

        /// <summary>
        /// updates an invoice line to the db and returns the new total value for the parent invoice request
        /// </summary>
        /// <param name="invoiceLine"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> UpdateInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct);

        /// <summary>
        /// get all invoice lines for an invoice request
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<InvoiceLine>> GetInvoiceLinesByInvoiceRequestId(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// gets a single AP invoice line by its id
        /// </summary>
        /// <param name="invoiceLineId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InvoiceLine> GetInvoiceLineByInvoiceLineId(Guid invoiceLineId, CancellationToken ct);

        /// <summary>
        /// gets a single AR invoice line by its id
        /// </summary>
        /// <param name="invoiceLineId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InvoiceLineAr> GetInvoiceLineArByInvoiceLineId(Guid invoiceLineId, CancellationToken ct);
    }
}
