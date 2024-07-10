using InvoiceLinesAdd;

using Microsoft.AspNetCore.Hosting;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests
{
    public class Sut : AppFixture<Program>
    {
        public AddInvoiceLineRequest AddInvoiceLineRequest { get; set; } = new();


        protected override async Task SetupAsync()
        {
            AddInvoiceLineRequest = new AddInvoiceLineRequest
            {
                Value = 12.12M,
                Description = "G00 - Gross value of claim",
	            FundCode = "FUND1",
	            MainAccount = "AR",
	            SchemeCode = "code1",
	            MarketingYear = "2020",
	            DeliveryBody = "DB1",
	            InvoiceRequestId = "333_YMALXCHG"
            };
        }

        protected override void ConfigureApp(IWebHostBuilder a)
        {
            // do host builder configuration here
            //a.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    config.(
            //      "appsettings.Custom.json",
            //      optional: true,
            //      reloadOnChange: false);
            //})
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInvoiceLineRepo, FakeInvoicelineRepo>();
            services.AddTransient<IReferenceDataRepo, ReferenceDataRepo>();
            services.AddTransient<IInvoiceRepo, InvoiceRepo>();
            services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        }

        protected override async Task TearDownAsync()
        {
            Client.Dispose();
        }
    }
}
