using System.Diagnostics.CodeAnalysis;

using InvoiceLines.Update;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace UpdateInvoiceLineAr
{
    /// <summary>
    /// updates an AR invoice line
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineArEndpoint : EndpointWithMapping<UpdateInvoiceLineArRequest, UpdateInvoiceLineArResponse, InvoiceLineAr>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<UpdateInvoiceLineArEndpoint> _arLogger;

        public UpdateInvoiceLineArEndpoint(
            ILogger<UpdateInvoiceLineArEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _arLogger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Put("/invoicelines/updatear");
        }

        public override async Task HandleAsync(UpdateInvoiceLineArRequest r, CancellationToken ct)
        {
            UpdateInvoiceLineArResponse response = new();

            try
            {
                InvoiceLineAr invoiceLine = await MapToEntityAsync(r, ct);

                var res = await _iInvoiceLineRepo.UpdateInvoiceLineAr(invoiceLine, ct);

                if (res != 0)
                {
                    response.InvoiceRequestValue = res;
                    response.InvoiceLine = invoiceLine;
                }
                else
                {
                    response.Message = "Error updating AR invoice line";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _arLogger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public sealed override async Task<InvoiceLineAr> MapToEntityAsync(UpdateInvoiceLineArRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLineAr());

            invoiceLine.Id = r.Id;
            invoiceLine.MarketingYear = r.MarketingYear;
            invoiceLine.DeliveryBodyCode = r.DeliveryBody;
            invoiceLine.Value = r.Value;
            invoiceLine.Description = r.Description;
            invoiceLine.FundCode = r.FundCode;
            invoiceLine.SchemeCode = r.SchemeCode;
            invoiceLine.MainAccount = r.MainAccount;
            invoiceLine.InvoiceRequestId = r.InvoiceRequestId;
            invoiceLine.DebtType = r.DebtType;

            return invoiceLine;
        }
    }
}