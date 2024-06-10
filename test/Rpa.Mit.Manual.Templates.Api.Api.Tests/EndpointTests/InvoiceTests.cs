using FakeItEasy;

using FastEndpoints;

using Invoices.Add;

using Microsoft.Extensions.Logging;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using Xunit;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class InvoiceTests()// : TestBase<MyApp>
    {
        [Fact]
        public async Task Invoice_Save_Success()
        {
            AddInvoiceRequest invoiceRequest = new AddInvoiceRequest
            {
                SchemeType = "EU (2014 - 2020 Program)",
                PaymentType = "GBP",
                AccountType = "AP",
                DeliveryBody = "EA",
                SecondaryQuestion = "Any question"
            };

            var fakeRepo = A.Fake<IInvoiceRepo>();
            A.CallTo(() => fakeRepo.AddInvoice(A<Invoice>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(true));

            var ep = Factory.Create<AddInvoiceEndpoint>(
                           A.Fake<ILogger<AddInvoiceEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            response.Invoice.Should().NotBeNull();
            response.Invoice.AccountType.Should().Match("AP");

            Assert.IsType<Guid>(response.Invoice.Id);
        }

        [Fact]
        public async Task Invoice_Save_Validation_Fail_Missing_DeliveryBody()
        {
            AddInvoiceRequest invoiceRequest = new AddInvoiceRequest
            {
                SchemeType = "EU (2014 - 2020 Program)",
                PaymentType = "GBP",
                AccountType = "AP",
                SecondaryQuestion = "Any question"
            };

            Invoice invoice = new Invoice();

            var fakeConfig = A.Fake<IInvoiceRepo>();
            A.CallTo(() => fakeConfig.AddInvoice(invoice, CancellationToken.None)).Returns(true);

            var ep = Factory.Create<AddInvoiceEndpoint>(
                           A.Fake<ILogger<AddInvoiceEndpoint>>(),
                           A.Fake<IInvoiceRepo>());

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            Assert.Equal(string.Empty, response.Message);
            Assert.IsType<Invoice>(response);
            Assert.NotNull(response.Invoice);
            Assert.IsType<Guid>(response.Invoice.Id);
        }
    }
}
