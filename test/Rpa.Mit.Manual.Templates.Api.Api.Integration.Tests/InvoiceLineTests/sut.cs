using InvoiceLines.Add;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceLineTests;
using Rpa.Mit.Manual.Templates.Api.Api.MitAzure;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests
{
    public class IltSut : AppFixture<Program.Program>
    {

        protected override void ConfigureServices(IServiceCollection services)
        {
            var desc = services.Single(s => s.ImplementationType == typeof(WorkerServiceBus<PaymentHubResponseRoot>));
            services.Remove(desc);

            var descriptor1 = services.Single(s => s.ImplementationType == typeof(ServiceBusProvider));
            services.Remove(descriptor1);

            var descriptor2 = services.Single(s => s.ImplementationType == typeof(NotificationHandler));
            services.Remove(descriptor2);

            services.AddSingleton<IServiceBusProvider, FakeServiceBusProvider>();
            services.AddTransient<IInvoiceLineRepo, FakeInvoicelineRepo>();
            services.AddTransient<IReferenceDataRepo, ReferenceDataRepo>();
            services.AddTransient<IInvoiceRepo, InvoiceRepo>();
            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
