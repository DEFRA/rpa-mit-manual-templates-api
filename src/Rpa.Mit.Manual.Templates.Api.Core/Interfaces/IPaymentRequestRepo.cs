using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentRequestRepo
    {
        Task<bool> AddPaymentRequest(PaymentRequest paymentRequest, CancellationToken ct);
    }
}
