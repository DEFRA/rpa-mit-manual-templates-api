using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLines.AddAp
{
    [ExcludeFromCodeCoverage]
    public sealed class AddInvoiceIineApEndpoint : EndpointWithMapping<AddInvoiceLineRequest, AddInvoiceLineResponse, InvoiceLine>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<AddInvoiceIineApEndpoint> _logger;

        public AddInvoiceIineApEndpoint(
            ILogger<AddInvoiceIineApEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo,
            IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            Post("/invoicelines/addap");
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

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public sealed override async Task<InvoiceLine> MapToEntityAsync(AddInvoiceLineRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLine());

            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsApReferenceData(ct);
            var descriptionQuery = r.MainAccount + "/" + r.SchemeCode + "/" + r.DeliveryBody;

            invoiceLine.MarketingYear = r.MarketingYear;
            invoiceLine.DeliveryBody = r.DeliveryBody;
            invoiceLine.Value = r.Value;
            invoiceLine.Description = chartOfAccounts.First(c => c.Code == descriptionQuery).Org;
            invoiceLine.FundCode = r.FundCode;
            invoiceLine.SchemeCode = r.SchemeCode;
            invoiceLine.MainAccount = r.MainAccount;
            invoiceLine.InvoiceRequestId = r.InvoiceRequestId;

            return invoiceLine;
        }
    }
}