using System.Diagnostics.CodeAnalysis;

using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.MitAzure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusProvider : IServiceBusProvider
    {
        public async Task SendInvoiceRequestJson(ServiceBusSender sender, string msg)
        {
            ServiceBusMessage message = new ServiceBusMessage(msg);

            await sender.SendMessageAsync(message);
        }
    }
}
