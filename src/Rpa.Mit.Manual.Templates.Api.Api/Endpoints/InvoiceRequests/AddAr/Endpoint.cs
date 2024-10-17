using InvoiceRequests.Add;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace AddInvoiceRequestAr
{
    internal sealed class Endpoint : EndpointWithMapping<AddArRequest, AddArResponse, InvoiceRequestAr>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<Endpoint> _logger;

        public Endpoint(
            ILogger<Endpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Post("/invoicerequests/addar");
        }

        public override async Task HandleAsync(AddArRequest r, CancellationToken ct)
        {
            AddArResponse response = new AddArResponse();

            try
            {
                InvoiceRequestAr invoiceRequest = await MapToEntityAsync(r, ct);

                if (await _iInvoiceRequestRepo.AddInvoiceRequestAr(invoiceRequest, ct))
                {
                    response.InvoiceRequest = invoiceRequest;
                }
                else
                {
                    ThrowError("Error adding new invoice request");
                }

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }

        public override async Task<InvoiceRequestAr> MapToEntityAsync(AddArRequest r, CancellationToken ct = default)
        {
            var invoiceRequest = await Task.FromResult(new InvoiceRequestAr());

            invoiceRequest.InvoiceRequestId = r.ClaimReferenceNumber + "-" + r.ClaimReference;
            invoiceRequest.FRN = r.FRN;
            invoiceRequest.SBI = r.SBI;
            invoiceRequest.Currency = r.Currency;
            invoiceRequest.Vendor = r.Vendor;
            invoiceRequest.AccountType = r.AccountType;
            invoiceRequest.AgreementNumber = r.AgreementNumber;
            invoiceRequest.Description = r.Description;
            invoiceRequest.InvoiceId = r.InvoiceId;
            invoiceRequest.Ledger = "AR";
            invoiceRequest.InvoiceRequestNumber = 1;  //TODO: this needs investigation
            invoiceRequest.Value = 0.00M;
            invoiceRequest.ClaimReference = r.ClaimReference;
            invoiceRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;

            return invoiceRequest;
        }
    }
}