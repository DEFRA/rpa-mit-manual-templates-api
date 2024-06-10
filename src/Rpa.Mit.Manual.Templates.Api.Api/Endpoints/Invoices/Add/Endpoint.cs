
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Enums;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.Add
{
    internal sealed class AddInvoiceEndpoint : EndpointWithMapping<AddInvoiceRequest, AddInvoiceResponse, Invoice>
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
                Invoice invoice = await MapToEntityAsync(invoiceRequest, ct);

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


        public override async Task<Invoice> MapToEntityAsync(AddInvoiceRequest r, CancellationToken ct = default)
        {
            var invoice = await Task.FromResult(new Invoice());

            invoice.AccountType = r.AccountType;
            invoice.DeliveryBody = r.DeliveryBody;
            invoice.SecondaryQuestion = r.SecondaryQuestion;
            invoice.SchemeType = r.SchemeType;
            invoice.Value = 0.00M;
            invoice.Status = InvoiceStatuses.New;

            return invoice;
        }
    }
}