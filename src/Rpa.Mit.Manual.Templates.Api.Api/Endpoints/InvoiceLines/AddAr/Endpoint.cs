using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace AddInvoiceIineAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceIineArEndpoint : EndpointWithMapping<AddInvoiceLineArRequest, AddInvoiceLineArResponse, InvoiceLineAr>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<AddInvoiceIineArEndpoint> _arLogger;

        public AddInvoiceIineArEndpoint(
                                        ILogger<AddInvoiceIineArEndpoint> arLogger,
                                        IInvoiceLineRepo iInvoiceLineRepo,
                                        IReferenceDataRepo iReferenceDataRepo)
        {
            _arLogger = arLogger;
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

                var res = await _iInvoiceLineRepo.AddInvoiceLineAr(invoiceLine, ct);

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
                _arLogger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public sealed override async Task<InvoiceLineAr> MapToEntityAsync(AddInvoiceLineArRequest r, CancellationToken ct = default)
        {
            var invoiceLine = await Task.FromResult(new InvoiceLineAr());

            var chartOfAccountsAr = await _iReferenceDataRepo.GetChartOfAccountsArReferenceData(ct);
            var descriptionQuery = r.MainAccount + "/" + r.SchemeCode + "/" + r.DeliveryBody;

            var description = chartOfAccountsAr.First(c => c.Code == descriptionQuery).Description;

            if (string.IsNullOrEmpty(description))
            {
                ThrowError("Invalid account/scheme/deliverybody combination");
            }

            invoiceLine.MarketingYear = r.MarketingYear;
            invoiceLine.DeliveryBodyCode = r.DeliveryBody;
            invoiceLine.Value = r.Value;
            invoiceLine.Description = description;
            invoiceLine.FundCode = r.FundCode;
            invoiceLine.SchemeCode = r.SchemeCode;
            invoiceLine.MainAccount = r.MainAccount;
            invoiceLine.InvoiceRequestId = r.InvoiceRequestId;
            invoiceLine.DebtType = r.DebtType;
            return invoiceLine;
        }
    }
}