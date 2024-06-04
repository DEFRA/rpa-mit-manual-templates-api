namespace GetPaymentTypes
{
    internal sealed class GetPaymentTypesEndpoint : EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            AllowAnonymous();
            Get("/paymenttypes/get");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}