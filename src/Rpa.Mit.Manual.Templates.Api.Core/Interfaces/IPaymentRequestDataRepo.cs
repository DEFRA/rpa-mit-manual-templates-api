using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IPaymentRequestDataRepo
    {
        Task<bool> AddPaymentRequest(PaymentRequest paymentRequest);
    }
}
