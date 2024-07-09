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
            var invoiceRequest = await Task.FromResult(new InvoiceRequest());

            invoiceRequest.InvoiceRequestId = r.InvoiceRequestId;
            invoiceRequest.MarketingYear = r.MarketingYear;
            invoiceRequest.InvoiceCorrectionReference = r.InvoiceCorrectionReference;
            invoiceRequest.Description = r.Description;
            invoiceRequest.FRN = r.FRN;
            invoiceRequest.Currency = r.Currency;
            invoiceRequest.Vendor = r.Vendor;
            invoiceRequest.AgreementNumber = r.AgreementNumber;
            invoiceRequest.ClaimReference = r.ClaimReference;
            invoiceRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;
            invoiceRequest.AgreementNumber = r.AgreementNumber.ToString();

            return invoiceRequest;
        }
    }
}