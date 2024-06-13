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
    }
}
