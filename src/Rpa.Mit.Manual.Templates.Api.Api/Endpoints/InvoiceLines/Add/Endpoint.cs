using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceIineEndpoint : EndpointWithMapping<AddInvoiceLineRequest, AddInvoiceLineResponse, InvoiceLine>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<AddInvoiceIineEndpoint> _logger;

        public AddInvoiceIineEndpoint(
            ILogger<AddInvoiceIineEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Get("/invoicelines/add");
        }

        public override async Task HandleAsync(AddInvoiceLineRequest r, CancellationToken ct)
        {
            AddInvoiceLineResponse response = new AddInvoiceLineResponse();

            try
            {
                InvoiceLine invoiceLIne = await MapToEntityAsync(r, ct);

                invoiceLIne.Id = Guid.NewGuid();

                if (await _iInvoiceLineRepo.AddInvoiceLine(invoiceLIne, ct))
                {
                    response.InvoiceLine = invoiceLIne;
                }
                else
                {
                    response.Message = "Error adding new payment request";
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

        public sealed override async Task<InvoiceLine> MapToEntityAsync(AddInvoiceLineRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLine());

            invoiceLine.MarketingYear = r.MarketingYear;
            invoiceLine.DeliveryBody = r.DeliveryBody;
            invoiceLine.Value = r.Value;
            invoiceLine.Description = r.Description;
            invoiceLine.PaymentRequestId = r.PaymentRequestId;

            return invoiceLine;
        }
    }
}