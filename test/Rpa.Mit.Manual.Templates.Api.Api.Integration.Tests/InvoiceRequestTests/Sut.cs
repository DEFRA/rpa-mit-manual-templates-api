using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceRequestTests
{
    public class Sut : AppFixture<Program.Program>
    {
        protected override void ConfigureApp(IWebHostBuilder a)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            var descriptor = services.Single(s => s.ImplementationType == typeof(WorkerServiceBus));
            services.Remove(descriptor);

            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
