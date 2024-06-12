using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

using Rpa.Mit.Manual.Templates.Api.Core.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    public class ServiceBusProvider : IServiceBusProvider
    {
        private readonly IOptions<AppSettings> _options;

        public ServiceBusProvider(IOptions<AppSettings> options)
        {
            _options = options;
        }

        public async Task SendMessageAsync(string queue, string msg)
        {
            await using var client = new ServiceBusClient(_options.Value.QueueConnectionString);

            ServiceBusSender sender = client.CreateSender(queue);

            ServiceBusMessage message = new ServiceBusMessage(msg.EncodeMessage());

            await sender.SendMessageAsync(message);
        }
    }
}
