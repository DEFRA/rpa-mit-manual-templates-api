using System.Diagnostics.CodeAnalysis;

using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

using Rpa.Mit.Manual.Templates.Api.Core.Services;

namespace Rpa.Mit.Manual.Templates.Api.Api.Azure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusProvider : IServiceBusProvider
    {
        private readonly IConfiguration _configuration;

        public ServiceBusProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string queue, string msg)
        {
            //TODO: need to get the correct client id here
            //var credential = new DefaultAzureCredentialOptions { ManagedIdentityClientId = "ae56873c-ba5d-4a68-9730-f39f77e3dd69" });
            //Guid tenantId = Guid.Parse("6f504113-6b64-43f2-ade9-242e057800");
            //TokenCredential tokenCredential = new VisualStudioCredential(new VisualStudioCredentialOptions { TenantId = "f2d4ac8e-632a-41ff-b83a-e5d39e7d095a" });

            if (string.IsNullOrEmpty(queue)) return;

            await using var client = new ServiceBusClient(_configuration.GetSection("ServiceBusNamespace").Value, new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = "ae56873c-ba5d-4a68-9730-f39f77e3dd69" }));
            ServiceBusSender sender = client.CreateSender(queue);
            ServiceBusMessage message = new ServiceBusMessage(msg.EncodeMessage());
            await sender.SendMessageAsync(message);
        }
    }
}
