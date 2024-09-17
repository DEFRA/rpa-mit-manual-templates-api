

using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLines.Update
{
    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineEndpoint : EndpointWithMapping<UpdateInvoiceLineRequest, UpdateInvoiceLineResponse, InvoiceLine>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<UpdateInvoiceLineEndpoint> _logger;

        public UpdateInvoiceLineEndpoint(
            ILogger<UpdateInvoiceLineEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Put("/invoicelines/update");
        }

        public override async Task HandleAsync(UpdateInvoiceLineRequest r, CancellationToken ct)
        {
            UpdateInvoiceLineResponse response = new();

            try
            {
                InvoiceLine invoiceLine = await MapToEntityAsync(r, ct);

                var res = await _iInvoiceLineRepo.UpdateInvoiceLine(invoiceLine, ct);

                if (res != 0)
                {
                    response.InvoiceRequestValue = res;
                    response.InvoiceLine = invoiceLine;
                }
                else
                {
                    response.Message = "Error updating invoice line";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public sealed override async Task<InvoiceLine> MapToEntityAsync(UpdateInvoiceLineRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLine());
            invoiceLine.Id = r.Id;
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