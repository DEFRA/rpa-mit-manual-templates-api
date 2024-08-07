using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

using Rpa.Mit.Manual.Templates.Api.Core.Services;

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

        public async Task SendMessageAsync(string msg)
        {
            if (string.IsNullOrEmpty(_options.PAYMENTHUB_CONNECTION) 
                || string.IsNullOrEmpty(_options.PAYMENTHUB_TOPIC)) return;

            await using var client = new ServiceBusClient(_options.PAYMENTHUB_CONNECTION);
            ServiceBusSender sender = client.CreateSender(_options.PAYMENTHUB_TOPIC);
            ServiceBusMessage message = new ServiceBusMessage(msg.EncodeMessage());
            await sender.SendMessageAsync(message);
        }
    }
}
