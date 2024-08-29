using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure.ServiceBusMessaging
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly ILogger _logger;
        private readonly PaymentHub _options;
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor? _processor = null;
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;

        public ServiceBusTopicSubscription(
            IOptions<PaymentHub> options,
            IInvoiceRequestRepo iInvoiceRequestRepo,
            ILogger<ServiceBusTopicSubscription> logger)
        {
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
            _options = options.Value;
            _logger = logger;
            _client = new ServiceBusClient(_options.CONNECTION);
        }


        public async Task PrepareFiltersAndHandleMessages()
        {
            var _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(
                _options.RESPONSE_TOPIC, 
                _options.RESPONSE_SUBSCRIPTION, 
                _serviceBusProcessorOptions);

            // add the handlers
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await _processor.StartProcessingAsync();
        }


        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            try
            {
                var phubResponse = args.Message.Body.ToObjectFromJson<PaymentHubResponseRoot>();

                if (null == phubResponse)
                {
                    return;
                }

                PaymentHubResponseForDatabase paymentHubResponseForDatabase = new PaymentHubResponseForDatabase
                {
                    invoicerequestid = phubResponse!.paymentRequest!.InvoiceRequestId,
                    paymenthubdateprocessed = DateTime.UtcNow,
                    error = phubResponse.error,
                    accepted = phubResponse.accepted
                };

                // update the database...
                if(await _iInvoiceRequestRepo.UpdateInvoiceRequestWithPaymentHubResponse(paymentHubResponseForDatabase))
                {

                    // if we have an error, we also need to email the originator of the data with the relevant data.

                    await args.CompleteMessageAsync(args.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
            }
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug("- ErrorSource: {ErrorSource}", arg.ErrorSource);
            _logger.LogDebug("- Entity Path: {EntityPath}", arg.EntityPath);
            _logger.LogDebug("- FullyQualifiedNamespace: {FullyQualifiedNamespace}", arg.FullyQualifiedNamespace);

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync();
            }

            if (_client != null)
            {
                await _client.DisposeAsync();
            }
        }

        public async Task CloseSubscriptionAsync()
        {
            await _processor!.CloseAsync();
        }
    }
}
