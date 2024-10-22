using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IInvoiceRequestRepo
    {
        /// <summary>
        /// adds an AP invoice request to the db
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct);

        /// <summary>
        /// adds an AR invoice request to the db
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddInvoiceRequestAr(InvoiceRequestAr invoiceRequest, CancellationToken ct);

        /// <summary>
        /// gets a single invoice (payment) request total value by adding up the values of all child invoice lines
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> GetInvoiceRequestValue(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// get a single AP invoice request by its id
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InvoiceRequest> GetInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// get a single AR invoice request by its id
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<InvoiceRequest> GetArInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// updates a single invoice request
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct);

        /// <summary>
        /// deletes an invoice request and all invoiceline children
        /// </summary>
        /// <param name="invoiceRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteInvoiceRequest(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// get all AP invoice requests for a parent invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsByInvoiceId(Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// get all AR invoice requests for a parent invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<InvoiceRequestAr>> GetArInvoiceRequestsByInvoiceId(Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// updates an invoice request with the response coming back from the payment hub after it has processed the request
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateInvoiceRequestWithPaymentHubResponse(PaymentHubResponseForDatabase paymentHubResponseForDatabase);

        /// <summary>
        /// gets a list of invoice requests that have errored in the payment hub.
        /// we need this so that we can send an email to the respective submitters of the data to alert them of the error and request a re-submit.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsThatHaveErroredInPaymentHub(CancellationToken ct);
    }
}
