using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceRequestsAr.GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetArByInvoiceIdEndpoint : Endpoint<Request, Response>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<GetArByInvoiceIdEndpoint> _logger;

        public GetArByInvoiceIdEndpoint(
            ILogger<GetArByInvoiceIdEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Get("/invoicerequests/getarbyinvoiceid");
        }


        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.InvoiceRequests = await _iInvoiceRequestRepo.GetArInvoiceRequestsByInvoiceId(r.InvoiceId, ct);

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}