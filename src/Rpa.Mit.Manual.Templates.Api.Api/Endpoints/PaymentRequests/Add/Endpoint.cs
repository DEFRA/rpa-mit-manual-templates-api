using Invoices.Add;
using Microsoft.Extensions.Options;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;

namespace PaymentsRequests.Add
{
    internal sealed class AddPaymentsRequestsEndpoint : Endpoint<Request, Response>
    {
        private readonly IPaymentRequestDataRepo _iPaymentRequestDataRepo;
        private readonly ILogger<AddPaymentsRequestsEndpoint> _logger;

        public AddPaymentsRequestsEndpoint(
            ILogger<AddPaymentsRequestsEndpoint> logger,
            IPaymentRequestDataRepo iPaymentRequestDataRepo)
        {
            _logger = logger;
            _iPaymentRequestDataRepo = iPaymentRequestDataRepo;
        }

        public override void Configure()
        {
            Post("/paymentrequest/add");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            Response response = new Response();

            try
            {
                response.Result = await _iPaymentRequestDataRepo.(r.PaymentRequest);

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