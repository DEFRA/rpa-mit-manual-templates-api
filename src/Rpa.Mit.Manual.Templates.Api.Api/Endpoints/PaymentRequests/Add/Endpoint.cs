
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Enums;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace PaymentsRequests.Add
{
    internal sealed class AddPaymentsRequestEndpoint : EndpointWithMapping<AddPaymentRequest, PaymentRequestResponse, PaymentRequest>
    {
        private readonly IPaymentRequestRepo _iPaymentRequestRepo;
        private readonly ILogger<AddPaymentsRequestEndpoint> _logger;

        public AddPaymentsRequestEndpoint(
            ILogger<AddPaymentsRequestEndpoint> logger,
            IPaymentRequestRepo iPaymentRequestRepo)
        {
            _logger = logger;
            _iPaymentRequestRepo = iPaymentRequestRepo;
        }

        public override void Configure()
        {
            Post("/paymentrequest/add");
        }

        public override async Task HandleAsync(AddPaymentRequest r, CancellationToken ct)
        {
            PaymentRequestResponse response = new PaymentRequestResponse();

            try
            {
                PaymentRequest paymentRequest = await MapToEntityAsync(r, ct);

                if(await _iPaymentRequestRepo.AddPaymentRequest(paymentRequest, ct))
                {
                    response.PaymentRequest = paymentRequest;
                }
                else
                {
                    response.Message = "Error adding new payment request";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }

        public override async Task<PaymentRequest> MapToEntityAsync(AddPaymentRequest r, CancellationToken ct = default)
        {
            var paymentRequest = await Task.FromResult(new PaymentRequest());

            paymentRequest.AccountType = r.AccountType;
            paymentRequest.FRN = r.FRN;
            paymentRequest.SBI = r.SBI;
            paymentRequest.Currency = r.Currency;
            paymentRequest.Vendor = r.Vendor;
            paymentRequest.AgreementNumber = r.AgreementNumber;
            paymentRequest.PaymentRequestNumber = r.PaymentRequestNumber;
            paymentRequest.MarketingYear = r.MarketingYear;
            paymentRequest.Description = r.Description;
            paymentRequest.DueDate = r.DueDate;

            return paymentRequest;
        }
    }
}