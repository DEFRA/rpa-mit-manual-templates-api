using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceEndpoint : Endpoint<Invoice, Response>
    {
        private readonly IInvoiceRepo _iInvoiceDataRepo;
        private readonly ILogger<AddInvoiceEndpoint> _logger;

        public AddInvoiceEndpoint(
            ILogger<AddInvoiceEndpoint> logger,
            IInvoiceRepo iInvoiceDataRepo)
        {
            _logger = logger;
            _iInvoiceDataRepo = iInvoiceDataRepo;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Post("/invoices/add");
        }

        public override async Task HandleAsync(Invoice invoice, CancellationToken ct)
        {
            invoice.Id = Guid.NewGuid();

            Response response = new Response();

            try
            {
                if(await _iInvoiceDataRepo.AddInvoice(invoice, ct))
                {
                    response.Invoice = invoice;
                }
                else
                {
                    response.Message = "Error adding new invoice";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}