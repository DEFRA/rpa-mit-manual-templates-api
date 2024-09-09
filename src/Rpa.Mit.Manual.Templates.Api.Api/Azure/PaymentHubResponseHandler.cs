using System.Diagnostics.CodeAnalysis;

using Microsoft.Identity.Client;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    /// <summary>
    /// handles responses from the payment hub servicebus topic
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class PaymentHubResponseHandler : IMessageHandler<PaymentHubResponseRoot>
    {
        private readonly ILogger<PaymentHubResponseHandler> _logger;
        private readonly IEmailService _iEmailService;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly IInvoiceRepo _iInvoiceRepo;

        public PaymentHubResponseHandler(
            IEmailService iEmailService,    
            IInvoiceRepo iInvoiceRepo,  
            IInvoiceRequestRepo iInvoiceRequestRepo,
            ILogger<PaymentHubResponseHandler> logger)
        {
            _iEmailService = iEmailService;
            _logger = logger;
            _iInvoiceRepo = iInvoiceRepo;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }


        public async Task<Task> HandleAsync(PaymentHubResponseRoot message, CancellationToken cancelToken)
        {
            try
            {
                var paymentHubResponseForDatabase = new PaymentHubResponseForDatabase
                {
                    invoicenumber = Guid.Parse(message!.paymentRequest!.invoiceNumber),
                    invoicerequestid = message!.paymentRequest!.InvoiceRequestId,
                    paymenthubdateprocessed = DateTime.UtcNow,
                    error = message.error,
                    accepted = message.accepted
                };

                await _iInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(paymentHubResponseForDatabase);

                // update the database...
                //if (await _iInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(paymentHubResponseForDatabase))
                //{
                    //if we have an error, we also need to email the originator of the data with the relevant data.
                if (!message.accepted)
                {
                    // first get the relevant user email
                    var email = await _iInvoiceRepo.GetInvoiceCreatorEmail(paymentHubResponseForDatabase.invoicenumber, cancelToken);

                    await _iEmailService.EmailPaymentHubError(email, message, cancelToken);
                }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // throw this so it it caught by the processor
                throw new Exception(ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
