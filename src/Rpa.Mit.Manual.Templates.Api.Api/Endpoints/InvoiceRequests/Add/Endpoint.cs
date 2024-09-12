
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests.Add;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceRequests.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequestEndpoint : EndpointWithMapping<AddInvoiceRequestRequest, AddInvoiceRequestResponse, InvoiceRequest>
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
            Post("/invoicerequests/add");
        }

        public override async Task HandleAsync(AddInvoiceRequestRequest r, CancellationToken ct)
        {
            AddInvoiceRequestResponse response = new AddInvoiceRequestResponse();

            try
            {
                InvoiceRequest invoiceRequest = await MapToEntityAsync(r, ct);

                if(await _iInvoiceRequestRepo.AddInvoiceRequest(invoiceRequest, ct))
                {
                    response.InvoiceRequest = invoiceRequest;
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

        public override async Task<InvoiceRequest> MapToEntityAsync(AddInvoiceRequestRequest r, CancellationToken ct = default)
        {
            var invoiceRequest = await Task.FromResult(new InvoiceRequest());

            invoiceRequest.InvoiceRequestId = r.ClaimReference + "-" + r.ClaimReferenceNumber;
            invoiceRequest.FRN = r.FRN;
            invoiceRequest.SBI = r.SBI;
            invoiceRequest.Currency = r.Currency;
            invoiceRequest.Vendor = r.Vendor;
            invoiceRequest.AccountType = r.AccountType;
            invoiceRequest.AgreementNumber = r.AgreementNumber;
            invoiceRequest.MarketingYear = r.MarketingYear;
            invoiceRequest.Description = r.Description;
            invoiceRequest.InvoiceId = r.InvoiceId;
            invoiceRequest.InvoiceRequestNumber = 1;  //TODO: this needs investigation
            invoiceRequest.Value = 0.00M;
            invoiceRequest.ClaimReference = r.ClaimReference;
            invoiceRequest.ClaimReferenceNumber = r.ClaimReferenceNumber;

            return invoiceRequest;
        }
    }
}