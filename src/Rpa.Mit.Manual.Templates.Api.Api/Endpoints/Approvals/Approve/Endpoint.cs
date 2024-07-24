using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace ApproveInvoice
{
    internal sealed class ApproveInvoiceEndpoint : Endpoint<Request, Response>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<ApproveInvoiceEndpoint> _logger;

        public ApproveInvoiceEndpoint(
            ILogger<ApproveInvoiceEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/approvals/approve");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            await SendAsync(new Response());
        }
    }
}