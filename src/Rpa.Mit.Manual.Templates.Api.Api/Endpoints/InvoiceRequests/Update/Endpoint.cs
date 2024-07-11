using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;


namespace UpdateInvoiceRequest
{
    internal sealed class UpdateInvoiceRequestEndpoint : EndpointWithMapping<UpdateInvoiceRequestRequest, UpdateInvoiceRequestResponse, InvoiceRequest>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<UpdateInvoiceRequestEndpoint> _logger;

        public UpdateInvoiceRequestEndpoint(
            ILogger<UpdateInvoiceRequestEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Put("/invoicerequests/update");
        }

        public override async Task HandleAsync(UpdateInvoiceRequestRequest r, CancellationToken ct)
        {
            UpdateInvoiceRequestResponse response = new UpdateInvoiceRequestResponse();
            response.Result = false;

            try
            {
                InvoiceRequest invoiceRequest = await MapToEntityAsync(r, ct);

                if (await _iInvoiceRequestRepo.UpdateInvoiceRequest(invoiceRequest, ct))
                {
                    response.Result = true;
                }
                else
                {
                    response.Message = "Error updating invoice request";
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

        public sealed override async Task<InvoiceRequest> MapToEntityAsync(UpdateInvoiceRequestRequest r, CancellationToken ct = default)
        {
            var invoiceRequest = await Task.FromResult(new InvoiceRequest());

            invoiceRequest.InvoiceRequestId = r.InvoiceRequestId;
            invoiceRequest.MarketingYear = r.MarketingYear;
            invoiceRequest.InvoiceCorrectionReference = r.InvoiceCorrectionReference;
            invoiceRequest.Description = r.Description;
            invoiceRequest.FRN = r.FRN;
            invoiceRequest.Currency = r.Currency;
            invoiceRequest.Vendor = r.Vendor;
            invoiceRequest.AgreementNumber = r.AgreementNumber;
            invoiceRequest.AccountType = r.AccountType;
            invoiceRequest.ClaimReference = r.ClaimReference;
            invoiceRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;
            invoiceRequest.AgreementNumber = r.AgreementNumber.ToString();

            return invoiceRequest;
        }
    }
}