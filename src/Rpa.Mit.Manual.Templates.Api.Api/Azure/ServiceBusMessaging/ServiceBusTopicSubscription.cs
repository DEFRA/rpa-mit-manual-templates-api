using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure.ServiceBusMessaging
{
    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly ILogger _logger;
        private readonly PaymentHub _options;
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor? _processor = null;

        public ServiceBusTopicSubscription(
            IOptions<PaymentHub> options,
            ILogger<ServiceBusTopicSubscription> logger)
        {
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

            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await _processor.StartProcessingAsync();
        }


        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<PaymentHubResponseRoot>();
          
            // process the data...

            await args.CompleteMessageAsync(args.Message);
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
