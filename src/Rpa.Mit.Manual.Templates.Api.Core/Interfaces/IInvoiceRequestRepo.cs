using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceRequestRepo
    {
        /// <summary>
        /// adds a payment request tp the db
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct);

        /// <summary>
        /// gets a single invoice (payment) request total value by adding up the values of all child invoice lines
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> GetInvoiceRequestValue(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// updates a single invoice request
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct);
    }
}
