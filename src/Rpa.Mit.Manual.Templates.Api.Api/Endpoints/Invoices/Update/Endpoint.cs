using System.Diagnostics.CodeAnalysis;

namespace Invoices.Update
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/invoices/update");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            await SendAsync(new Response());
        }
    }
}