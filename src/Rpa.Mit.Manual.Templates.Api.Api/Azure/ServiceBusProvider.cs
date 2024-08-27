using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.MitAzure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusProvider : IServiceBusProvider
    {
        //private readonly PaymentHub _options;

        public ServiceBusProvider()
        {
         //   _options = options.Value;
        }

        public async Task SendInvoiceRequestJson(ServiceBusSender sender, string msg)
        {
            //await using var client = new ServiceBusClient(_options.CONNECTION);
            //ServiceBusSender sender = client.CreateSender(_options.TOPIC);

            ServiceBusMessage message = new ServiceBusMessage(msg);

            await sender.SendMessageAsync(message);
        }
    }
}
