using System.Diagnostics.CodeAnalysis;

namespace Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class Endpoint : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Get("/invoicelines/add");
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
        {
            await SendAsync(new Response(), cancellation: ct);
        }
    }
}