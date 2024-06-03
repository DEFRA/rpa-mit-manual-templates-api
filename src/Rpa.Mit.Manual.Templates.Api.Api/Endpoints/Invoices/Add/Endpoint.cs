using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.Add
{
    internal sealed class AddInvoiceEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly AzureAd _options;
        private readonly IInvoiceDataRepo _iInvoiceDataRepo;

        public AddInvoiceEndpoint(IOptions<AzureAd> options, IInvoiceDataRepo iInvoiceDataRepo)
        {
            _options = options.Value;
            _iInvoiceDataRepo = iInvoiceDataRepo;
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