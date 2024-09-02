

using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceLineTests
{
    internal class FakeServiceBusProvider : IServiceBusProvider
    {
        public Task SendInvoiceRequestJson(string msg)
        {
            //var client = new ServiceBusClient();

            return null;// Task;//.FromResult(client.CreateSender("MyTestTopic"));
        }
    }
}
