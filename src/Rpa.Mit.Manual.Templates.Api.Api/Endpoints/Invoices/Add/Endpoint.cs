using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices.Add;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.Add
{
    // [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceEndpoint : Endpoint<AddInvoiceRequest, AddInvoiceResponse, InvoiceMapper>
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

        /// <summary>
        /// saves a new invoice header
        /// </summary>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AddInvoiceRequest invoiceRequest, CancellationToken ct)
        {
            AddInvoiceResponse response = new();

            try
            {
                Invoice invoice = Map.ToEntity(invoiceRequest);

                invoice.Id = Guid.NewGuid();
                invoice.Created = DateTime.UtcNow;
                invoice.CreatedBy = "aylmer.carson";

                if (await _iInvoiceDataRepo.AddInvoice(invoice, ct))
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