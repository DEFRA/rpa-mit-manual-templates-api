using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.MitAzure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusProvider : IServiceBusProvider
    {
        private readonly PaymentHub _options;

        public ServiceBusProvider(IOptions<PaymentHub> options)
        {
            _options = options.Value;
        }

        public async Task SendInvoiceRequestJson(ServiceBusSender sender, string msg)
        {
            //await using var client = new ServiceBusClient(_options.CONNECTION);
            //ServiceBusSender sender = client.CreateSender(_options.TOPIC);

            ServiceBusMessage message = new ServiceBusMessage(msg);

            await sender.SendMessageAsync(message);
        }

        //public async Task IServiceBusProvider.GetPaymentHubResponses(ServiceBusSender sender, string msg)
        //{
        //    ServiceBusClient serviceBusClient = new ServiceBusClient(_options.CONNECTION);
        //    ServiceBusProcessor _ordersProcessor = serviceBusClient.CreateProcessor(_options.RESPONSE_TOPIC);
        //    _ordersProcessor.ProcessMessageAsync += PaymentHubResponseHandler;
        //    _ordersProcessor.ProcessErrorAsync += PizzaItemErrorHandler;
        //    await _ordersProcessor.StartProcessingAsync();

        //    throw new NotImplementedException();
        //}
    }
}
