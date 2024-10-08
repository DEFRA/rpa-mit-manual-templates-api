using InvoiceLines.AddAp;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace AddInvoiceIineAr
{
    internal sealed class AddInvoiceIineArEndpoint : EndpointWithMapping<AddInvoiceLineArRequest, AddInvoiceLineArResponse, InvoiceLineAr>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<AddInvoiceIineArEndpoint> _logger;

        public AddInvoiceIineArEndpoint(
                                        ILogger<AddInvoiceIineArEndpoint> logger,
                                        IInvoiceLineRepo iInvoiceLineRepo,
                                        IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            Post("/invoicelines/addar");
        }

        public override async Task HandleAsync(AddInvoiceLineArRequest r, CancellationToken ct)
        {
            AddInvoiceLineArResponse response = new AddInvoiceLineArResponse();

            try
            {
                InvoiceLineAr invoiceLine = await MapToEntityAsync(r, ct);

                invoiceLine.Id = Guid.NewGuid();

                var res = await _iInvoiceLineRepo.AddInvoiceLine(invoiceLine, ct);

                if (res != 0)
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

        public sealed override async Task<InvoiceLineAr> MapToEntityAsync(AddInvoiceLineArRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLineAr());

            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsArReferenceData(ct);
            var descriptionQuery = r.MainAccount + "/" + r.SchemeCode + "/" + r.DeliveryBody;

            invoiceLine.MarketingYear = r.MarketingYear;
            invoiceLine.DeliveryBody = r.DeliveryBody;
            invoiceLine.Value = r.Value;
            invoiceLine.Description = chartOfAccounts.First(c => c.Code == descriptionQuery).Org;
            invoiceLine.FundCode = r.FundCode;
            invoiceLine.SchemeCode = r.SchemeCode;
            invoiceLine.MainAccount = r.MainAccount;
            invoiceLine.InvoiceRequestId = r.InvoiceRequestId;
            invoiceLine.DebtType = r.DebtType;
            return invoiceLine;
        }
    }
}