using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentRequestRepo
    {
        /// <summary>
        /// adds a payment request tp the db
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> AddPaymentRequest(PaymentRequest paymentRequest, CancellationToken ct);

        /// <summary>
        /// gets a single invoice (payment) request total value by adding up the values of all child invoice lines
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> GetInvoiceRequestValue(string invoiceRequestId, CancellationToken ct);

        /// <summary>
        /// updates a single invoice request
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateInvoiceRequest(PaymentRequest paymentRequest, CancellationToken ct);
    }
}
