using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetByInvoiceLineIdAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLineArByInvoiceLineIdEndpoint : Endpoint<GetInvoiceLineByIdArRequest, GetInvoiceLineByIdArResponse>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<InvoiceLineArByInvoiceLineIdEndpoint> _arLogger;


        public InvoiceLineArByInvoiceLineIdEndpoint(
            ILogger<InvoiceLineArByInvoiceLineIdEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _arLogger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Get("/invoicelines/getbyinvoicelineidar");
        }

        public override async Task HandleAsync(GetInvoiceLineByIdArRequest r, CancellationToken ct)
        {
            var response = new GetInvoiceLineByIdArResponse();

            try
            {
                response.InvoiceLine = await _iInvoiceLineRepo.GetInvoiceLineArByInvoiceLineId(r.InvoiceLineId, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _arLogger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}