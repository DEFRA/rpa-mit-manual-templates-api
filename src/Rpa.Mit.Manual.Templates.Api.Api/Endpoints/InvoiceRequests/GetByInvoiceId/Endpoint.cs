using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceRequests.GetByInvoiceId
{
    internal sealed class InvoiceRequestsByInvoiceIdEndpoint : Endpoint<InvoiceRequestsGetByInvoiceIdRequest, InvoiceRequestsGetByInvoiceIdResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<InvoiceRequestsByInvoiceIdEndpoint> _logger;

        public InvoiceRequestsByInvoiceIdEndpoint(
            ILogger<InvoiceRequestsByInvoiceIdEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Get("/invoicerequests/getbyinvoiceid");
        }

        public override async Task HandleAsync(InvoiceRequestsGetByInvoiceIdRequest r, CancellationToken ct)
        {
            var response = new InvoiceRequestsGetByInvoiceIdResponse();

            try
            {
                response.InvoiceRequests = await _iInvoiceRequestRepo.GetInvoiceRequestsByInvoiceId(r.InvoiceId, ct);

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