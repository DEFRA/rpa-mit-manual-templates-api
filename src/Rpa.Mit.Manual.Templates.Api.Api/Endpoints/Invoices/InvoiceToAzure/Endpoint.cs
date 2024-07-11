using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Invoices.ToAzure
{
    internal sealed class InvoiceToAzureEndpoint : Endpoint<InvoiceToAzureRequest, InvoiceToAzureResponse>
    {
        private readonly IInvoiceRepo _iInvoiceDataRepo;
        private readonly ILogger<InvoiceToAzureEndpoint> _logger;
        private readonly IEventQueueService _iEventQueueService;

        public InvoiceToAzureEndpoint(
            ILogger<InvoiceToAzureEndpoint> logger,
            IEventQueueService iEventQueueService,
            IInvoiceRepo iInvoiceDataRepo)
        {
            _logger = logger;
            _iEventQueueService = iEventQueueService;
            _iInvoiceDataRepo = iInvoiceDataRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/invoices/publishtoazure");
        }

        public override async Task HandleAsync(InvoiceToAzureRequest r, CancellationToken ct)
        {
            InvoiceToAzureResponse response = new InvoiceToAzureResponse();

            try
            {
                var invoice = await _iInvoiceDataRepo.GetInvoiceForAzure(r.InvoiceId, ct);

                if (invoice == null)
                {
                    await SendAsync(response, cancellation: ct);
                }
                else
                {
                    // go ahead with sending to azure
                    await _iEventQueueService.CreateMessage(invoice, "Invoice created");
                }

                await SendAsync(new InvoiceToAzureResponse(), cancellation: ct);
            }
            catch (ServiceBusException ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
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