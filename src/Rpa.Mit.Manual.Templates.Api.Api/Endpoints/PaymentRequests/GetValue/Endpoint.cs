
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetInvoiceRequest
{
    internal sealed class GetInvoiceRequestValueEndpoint : Endpoint<GetInvoiceRequestValueRequest, GetInvoiceRequestValueResponse>
    {
        private readonly IPaymentRequestRepo _iPaymentRequestRepo;
        private readonly ILogger<GetInvoiceRequestValueEndpoint> _logger;

        public GetInvoiceRequestValueEndpoint(
            ILogger<GetInvoiceRequestValueEndpoint> logger,
            IPaymentRequestRepo iPaymentRequestRepo)
        {
            _logger = logger;
            _iPaymentRequestRepo = iPaymentRequestRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/invoicerequest/getvalue");
        }

        public override async Task HandleAsync(GetInvoiceRequestValueRequest r, CancellationToken ct)
        {
            var response = new GetInvoiceRequestValueResponse();

            try
            {
                response.InvoiceRequestValue = await _iPaymentRequestRepo.GetInvoiceRequestValue(r.InvoiceRequestId, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}