using Microsoft.AspNetCore.Hosting;

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
            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }
    }
}
