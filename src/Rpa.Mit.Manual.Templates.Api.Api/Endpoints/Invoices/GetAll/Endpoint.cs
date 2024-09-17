using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Invoices.GetAll
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetAllInvoicesEndpoint : EndpointWithoutRequest<GetAllInvoicesResponse>
    {
        private readonly IInvoiceRepo _iInvoiceRepo;
        private readonly ILogger<GetAllInvoicesEndpoint> _logger;

        public GetAllInvoicesEndpoint(
            ILogger<GetAllInvoicesEndpoint> logger,
            IInvoiceRepo iInvoiceRepo)
        {
            _logger = logger;
            _iInvoiceRepo = iInvoiceRepo;
        }

        public override void Configure()
        {
            Get("/invoices/getall");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new GetAllInvoicesResponse();

            try
            {
                response.Invoices = await _iInvoiceRepo.GetAllInvoices(ct);

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}