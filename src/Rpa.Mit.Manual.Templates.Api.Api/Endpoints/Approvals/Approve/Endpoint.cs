using System.Diagnostics.CodeAnalysis;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceEndpoint : Endpoint<Request, Response>
    {

        public ApproveInvoiceEndpoint()
        {
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