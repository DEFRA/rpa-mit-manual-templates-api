using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;

namespace Invoices.Add
{
    internal sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        private readonly AzureAd _options;

        public Endpoint(IOptions<AzureAd> options)
        {
            _options = options.Value;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/invoices/add");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            var t = _options.TenantId;

            await SendAsync(new Response());
        }
    }
}