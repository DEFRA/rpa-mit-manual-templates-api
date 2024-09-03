using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.MitAzure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusProvider : IServiceBusProvider
    {

        private readonly ServiceBusSender _serviceBusSender;

        public ServiceBusProvider(ServiceBusSender serviceBusSender) => _serviceBusSender = serviceBusSender;

        public async Task SendInvoiceRequestJson(string msg)
        {
            ServiceBusMessage message = new ServiceBusMessage(msg);

            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
