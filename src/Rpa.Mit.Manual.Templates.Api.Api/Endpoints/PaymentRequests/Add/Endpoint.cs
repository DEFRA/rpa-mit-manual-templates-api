using Invoices.Add;
using Microsoft.Extensions.Options;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;

namespace PaymentsRequests.Add
{
    internal sealed class AddPaymentsRequestsEndpoint : Endpoint<Request, Response>
    {
        private readonly IPaymentRequestRepo _iPaymentRequestRepo;
        private readonly ILogger<AddPaymentsRequestsEndpoint> _logger;

        public AddPaymentsRequestsEndpoint(
            ILogger<AddPaymentsRequestsEndpoint> logger,
            IPaymentRequestRepo iPaymentRequestRepo)
        {
            _logger = logger;
            _iPaymentRequestRepo = iPaymentRequestRepo;
        }

        public override void Configure()
        {
            Post("/paymentrequest/add");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            Response response = new Response();

            try
            {
                response.Result = await _iPaymentRequestRepo.AddPaymentRequest(r.PaymentRequest, ct);

                await SendAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Message = ex.Message;

                await SendAsync(response);
            }
        }
    }
}