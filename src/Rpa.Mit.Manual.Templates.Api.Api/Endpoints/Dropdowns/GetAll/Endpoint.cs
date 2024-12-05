namespace Dropdowns.GetAll
{
    internal sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            Post("dropdowns/getall");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}