using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetByInvoiceLineId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLineByInvoiceLineIdEndpoint : Endpoint<GetInvoiceLineByIdRequest, GetInvoiceLineByIdResponse>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<InvoiceLineByInvoiceLineIdEndpoint> _logger;

        public InvoiceLineByInvoiceLineIdEndpoint(
            ILogger<InvoiceLineByInvoiceLineIdEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Get("/invoicelines/getbyinvoicelineid");
        }

        public override async Task HandleAsync(GetInvoiceLineByIdRequest r, CancellationToken ct)
        {
            var response = new GetInvoiceLineByIdResponse();

            try
            {
                response.InvoiceLine = await _iInvoiceLineRepo.GetInvoiceLineByInvoiceLineId(r.InvoiceLineId, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}