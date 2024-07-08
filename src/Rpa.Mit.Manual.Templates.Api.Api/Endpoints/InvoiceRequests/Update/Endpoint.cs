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
            Post("/invoicerequests/update");
        }

        public override async Task HandleAsync(UpdateInvoiceRequestRequest r, CancellationToken c)
        {
            await SendAsync(new UpdateInvoiceRequestResponse());
        }

        public sealed override async Task<InvoiceRequest> MapToEntityAsync(UpdateInvoiceRequestRequest r, CancellationToken ct = default)
        {
            var paymentRequest = await Task.FromResult(new InvoiceRequest());

            paymentRequest.InvoiceRequestId = r.InvoiceRequestId;
            paymentRequest.MarketingYear = r.MarketingYear;
            paymentRequest.InvoiceCorrectionReference = r.InvoiceCorrectionReference;
            paymentRequest.Description = r.Description;
            paymentRequest.FRN = r.FRN;
            paymentRequest.Currency = r.Currency;
            paymentRequest.Vendor = r.Vendor;
            paymentRequest.AgreementNumber = r.AgreementNumber;
            paymentRequest.ClaimReference = r.ClaimReference;
            paymentRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;
            paymentRequest.AgreementNumber = r.AgreementNumber.ToString();

            return paymentRequest;
        }
    }
}