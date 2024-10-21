using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetArByInvoiceId
{
    internal sealed class Endpoint : Endpoint<Request, Response>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(
            ILogger<Endpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Get("/invoicerequests/getarbyinvoiceid");
        }


        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}