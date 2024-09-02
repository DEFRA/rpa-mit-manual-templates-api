using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Api.MitAzure;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceRequestTests
{
    public class Sut : AppFixture<Program.Program>
    {
        protected override void ConfigureApp(IWebHostBuilder a)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {

            var desc = services.Single(s => s.ImplementationType == typeof(WorkerServiceBus<PaymentHubResponseRoot>));
            services.Remove(desc);

            var descriptor1 = services.Single(s => s.ImplementationType == typeof(ServiceBusProvider));
            services.Remove(descriptor1);

            var descriptor2 = services.Single(s => s.ImplementationType == typeof(NotificationHandler));
            services.Remove(descriptor2);

            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
