using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.Add
{
    internal sealed class AddInvoiceEndpoint : Endpoint<Request, Response>
    {
        private readonly AzureAd _options;
        private readonly IInvoiceRepo _iInvoiceDataRepo;
        private readonly ILogger<AddInvoiceEndpoint> _logger;

        public AddInvoiceEndpoint(
            IOptions<AzureAd> options, 
            ILogger<AddInvoiceEndpoint> logger,
            IInvoiceRepo iInvoiceDataRepo)
        {
            _options = options.Value;
            _logger = logger;
            _iInvoiceDataRepo = iInvoiceDataRepo;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/invoices/add");
        }

        public override async Task HandleAsync(Request request, CancellationToken c)
        {
            request.Invoice.Id = Guid.NewGuid();

            Response response = new Response();

            var tenantId = _options.TenantId;

            try
            {
                if(await _iInvoiceDataRepo.AddInvoice(request.Invoice))
                {
                    response.Invoice = request.Invoice;
                }
                else
                {
                    response.Message = "Error adding new invoice";
                }

                await SendAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                response.Message = ex.Message;

                await SendAsync(response);
            }
        }
    }
}