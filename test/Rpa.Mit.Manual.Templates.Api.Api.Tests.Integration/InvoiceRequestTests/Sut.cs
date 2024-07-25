using Microsoft.AspNetCore.Hosting;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceRequestTests
{
    public class Sut : AppFixture<Program>
    {
        protected override void ConfigureApp(IWebHostBuilder a)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
