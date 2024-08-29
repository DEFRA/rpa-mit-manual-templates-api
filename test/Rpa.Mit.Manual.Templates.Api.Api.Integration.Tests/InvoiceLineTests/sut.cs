using InvoiceLines.Add;

using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests
{
    public class Sut : AppFixture<Program.Program>
    {
        public AddInvoiceLineRequest AddInvoiceLineRequest = new();

        protected override void ConfigureServices(IServiceCollection services)
        {
            var descriptor = services.Single(s => s.ImplementationType == typeof(WorkerServiceBus));
            services.Remove(descriptor);

            services.AddTransient<IInvoiceLineRepo, FakeInvoicelineRepo>();
            services.AddTransient<IReferenceDataRepo, ReferenceDataRepo>();
            services.AddTransient<IInvoiceRepo, InvoiceRepo>();
            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
