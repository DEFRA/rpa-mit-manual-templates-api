
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace PaymentsRequests.Add
{
    internal sealed class AddInvoiceRequestEndpoint : EndpointWithMapping<AddInvoiceRequest, AddInvoiceRequestResponse, InvoiceRequest>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<AddInvoiceRequestEndpoint> _logger;

        public AddInvoiceRequestEndpoint(
            ILogger<AddInvoiceRequestEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/paymentrequest/add");
        }

        public override async Task HandleAsync(AddInvoiceRequest r, CancellationToken ct)
        {
            AddInvoiceRequestResponse response = new AddInvoiceRequestResponse();

            try
            {
                InvoiceRequest paymentRequest = await MapToEntityAsync(r, ct);

                if(await _iInvoiceRequestRepo.AddInvoiceRequest(paymentRequest, ct))
                {
                    response.PaymentRequest = paymentRequest;
                }
                else
                {
                    response.Message = "Error adding new payment request";
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

        public override async Task<InvoiceRequest> MapToEntityAsync(AddInvoiceRequest r, CancellationToken ct = default)
        {
            var paymentRequest = await Task.FromResult(new InvoiceRequest());

            paymentRequest.InvoiceRequestId = r.PaymentRequestId;
            paymentRequest.FRN = r.FRN;
            paymentRequest.SBI = r.SBI;
            paymentRequest.Currency = r.Currency;
            paymentRequest.Vendor = r.Vendor;
            paymentRequest.AgreementNumber = r.AgreementNumber;
            paymentRequest.MarketingYear = r.MarketingYear;
            paymentRequest.Description = r.Description;
            paymentRequest.InvoiceId = r.InvoiceId;
            paymentRequest.PaymentRequestNumber = 1;  //TODO: this needs investigation
            paymentRequest.Value = 0.00M;
            paymentRequest.ClaimReference = r.ClaimReference;
            paymentRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;

            return paymentRequest;
        }
    }
}