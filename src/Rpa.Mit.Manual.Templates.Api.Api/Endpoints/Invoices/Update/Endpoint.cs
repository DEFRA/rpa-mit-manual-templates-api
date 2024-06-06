namespace Invoices.Update
{
    internal sealed class Endpoint : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/invoices/update");
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}