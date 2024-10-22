using System.Diagnostics.CodeAnalysis;

using GetByInvoiceRequestId;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetArByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetArByInvoiceRequestIdEndpoint : Endpoint<Request, GetArByInvoiceRequestIdResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<GetArByInvoiceRequestIdEndpoint> _logger;

        public GetArByInvoiceRequestIdEndpoint(
            ILogger<GetArByInvoiceRequestIdEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Get("/invoicerequests/getarbyid");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            var response = new GetArByInvoiceRequestIdResponse();

            try
            {
                response.InvoiceRequest = await _iInvoiceRequestRepo.GetInvoiceRequestByInvoiceRequestId(r.InvoiceRequestId, ct);

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