namespace Get
{
    internal sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            Post("/referencedata/get");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}