
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLines.GetByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLineByInvoiceRequestIdEndpoint : Endpoint<InvoiceLinesGetByInvoiceRequestIdRequest, InvoiceLinesGetByInvoiceRequestIdResponse>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<InvoiceLineByInvoiceRequestIdEndpoint> _logger;

        public InvoiceLineByInvoiceRequestIdEndpoint(
            ILogger<InvoiceLineByInvoiceRequestIdEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Get("/invoicelines/getbyinvoicerequestid");
        }

        public override async Task HandleAsync(InvoiceLinesGetByInvoiceRequestIdRequest r, CancellationToken ct)
        {
            var response = new InvoiceLinesGetByInvoiceRequestIdResponse();

            try
            {
                response.InvoiceLines = await _iInvoiceLineRepo.GetInvoiceLinesByInvoiceRequestId(r.InvoiceRequestId, ct);

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