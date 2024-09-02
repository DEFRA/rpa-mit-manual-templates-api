using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Rpa.Mit.Manual.Templates.Api.Api.Integration.Tests.InvoiceRequestTests
{
    public class Tests : IClassFixture<WebApplicationFactory<Program.Program>>
    {
        private readonly WebApplicationFactory<Program.Program> _factory;

        public Tests(WebApplicationFactory<Program.Program> factory)
        {
            _factory = factory;
        }

        IConfiguration? Configuration { get; set; }

        [Fact]
        public async Task Add_InvoiceRequest_FailsWith401()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/referencedata/getall");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            var builder = new ConfigurationBuilder().AddUserSecrets<Tests>();

            Configuration = builder.Build();

            var username = Configuration["ApiUsername"];

            //var addInvoiceRequestRequest = new AddInvoiceRequestRequest
            //{
            //    InvoiceRequestId = "1LTZBQGJu",
            //    InvoiceId = new Guid("5e95723c-eb51-47e4-abb8-3881d2a2afff"),
            //    SourceSystem= "Manual",
            //    FRN= "1234567890"
            //    //SBI= null,
            //    //MarketingYear= "2017",
            //    //PaymentRequestNumber= 1,
            //    //AgreementNumber= "1",
            //    //Currency= "GBP",
            //    //DueDate= "2024-07-13",
            //    //Value= 0,
            //    //InvoiceLines= [],
            //    //AccountType= "AP",
            //    //OriginalInvoiceNumber= "",
            //    //OriginalSettlementDate= "0001-01-01T00:00:00",
            //    //RecoveryDate= "0001-01-01T00:00:00",
            //    //InvoiceCorrectionReference= "",
            //    //Vendor= "",
            //    //Description= "Test",
            //    //AllErrors= { },
            //    //Errors= { },
            //    //ErrorPath= ""
            //};

            //var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceRequestEndpoint, AddInvoiceRequestRequest, AddInvoiceRequestResponse>(
            //    addInvoiceRequestRequest);

            //TODO: needs fixing
            //rsp.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //res.InvoiceRequestValue.Should().Be(12.12M);
            //res.InvoiceLine.Description.Should().Be("G00 - Gross value of claim");
            //res.InvoiceLine.FundCode.Should().Be("FUND1");
            //res.InvoiceLine.MainAccount.Should().Be("AR");
            //res.InvoiceLine.SchemeCode.Should().Be("code1");
            //res.InvoiceLine.MarketingYear.Should().Be("2020");
            //res.InvoiceLine.DeliveryBody.Should().Be("DB1");
            //res.InvoiceLine.InvoiceRequestId.Should().Be("333_YMALXCHG");

            //res.InvoiceLine.Id.Should().NotBe("00000000-0000-0000-0000-000000000001");
        }
    }
}
