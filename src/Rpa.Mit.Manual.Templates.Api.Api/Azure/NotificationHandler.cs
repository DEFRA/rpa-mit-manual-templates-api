using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// handles responses from the payment hub
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class NotificationHandler : IMessageHandler<PaymentHubResponseRoot>
    {
        private readonly ILogger<NotificationHandler> _logger;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;

        public NotificationHandler(
            IInvoiceRequestRepo iInvoiceRequestRepo,
            ILogger<NotificationHandler> logger)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }


        public async Task<Task> HandleAsync(PaymentHubResponseRoot message, CancellationToken cancelToken)
        {
            try
            {
                PaymentHubResponseForDatabase paymentHubResponseForDatabase = new PaymentHubResponseForDatabase
                {
                    invoicerequestid = message!.paymentRequest!.InvoiceRequestId,
                    paymenthubdateprocessed = DateTime.UtcNow,
                    error = message.error,
                    accepted = message.accepted
                };

                // update the database...
                if (await _iInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(paymentHubResponseForDatabase))
                {

                    // if we have an error, we also need to email the originator of the data with the relevant data.

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
