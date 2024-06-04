namespace GetSchemeTypes
{
    internal sealed class GetSchemeTypesEndpoint : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            AllowAnonymous();
            Get("/schemetypes/get");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}