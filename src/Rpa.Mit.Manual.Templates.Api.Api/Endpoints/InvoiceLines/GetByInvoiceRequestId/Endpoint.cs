using InvoiceLines.Update;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLines.GetByInvoiceRequestId
{
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
            // temp allow anon
            AllowAnonymous();
            Get("/invoicelines/getbyinvoicerequestid");
        }

        public override async Task HandleAsync(InvoiceLinesGetByInvoiceRequestIdRequest r, CancellationToken c)
        {
            await SendAsync(new InvoiceLinesGetByInvoiceRequestIdResponse());
        }
    }
}