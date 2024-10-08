using System.Diagnostics.CodeAnalysis;

using InvoiceLinesArByInvoiceRequestIdEndpoint.InvoiceLinesAr.GetByInvoiceRequestId;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLinesArByInvoiceRequestIdEndpoint
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLineByInvoiceRequestIdEndpoint : Endpoint<InvoiceLinesArGetByInvoiceRequestIdRequest, InvoiceLinesArGetByInvoiceRequestIdResponse>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<InvoiceLineByInvoiceRequestIdEndpoint> _arLogger;

        public InvoiceLineByInvoiceRequestIdEndpoint(
            ILogger<InvoiceLineByInvoiceRequestIdEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _arLogger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Get("/invoicelines/getbyinvoicerequestidar");
        }

        public override async Task HandleAsync(InvoiceLinesArGetByInvoiceRequestIdRequest r, CancellationToken c)
        {
            var response = new InvoiceLinesArGetByInvoiceRequestIdResponse();

            try
            {
                response.InvoiceLines = await _iInvoiceLineRepo.GetInvoiceLinesArByInvoiceRequestId(r.InvoiceRequestId, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _arLogger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}