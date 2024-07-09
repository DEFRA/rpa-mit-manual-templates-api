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
            Post("/invoicelines/add");
        }

        public override async Task HandleAsync(AddInvoiceLineRequest r, CancellationToken ct)
        {
            AddInvoiceLineResponse response = new AddInvoiceLineResponse();

            try
            {
                InvoiceLine invoiceLine = await MapToEntityAsync(r, ct);

                invoiceLine.Id = Guid.NewGuid();

                var res = await _iInvoiceLineRepo.AddInvoiceLine(invoiceLine, ct);
                
                if(res != 0)
                {
                    response.InvoiceRequestValue = res;
                    response.InvoiceLine = invoiceLine;
                }
                else
                {
                    response.Message = "Error adding new invoice line";
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
            invoiceLine.FundCode = r.FundCode;
            invoiceLine.SchemeCode = r.SchemeCode;
            invoiceLine.MainAccount = r.MainAccount;
            invoiceLine.InvoiceRequestId = r.InvoiceRequestId;

            return invoiceLine;
        }
    }
}