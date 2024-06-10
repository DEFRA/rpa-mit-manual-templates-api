using FakeItEasy;

using FastEndpoints;

using Invoices.Add;

using Microsoft.Extensions.Logging;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using Xunit;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class InvoiceTests()
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
            response.Invoice?.AccountType.Should().Match("AP");

            Assert.IsType<Guid>(response.Invoice?.Id);
        }

        [Fact]
        public async Task Invoice_Save_Fail()
        {
            AddInvoiceRequest invoiceRequest = new AddInvoiceRequest
            {
                SchemeType = "EU (2014 - 2020 Program)",
                PaymentType = "GBP",
                AccountType = "AP",
                SecondaryQuestion = "Any question"
            };

            var fakeRepo = A.Fake<IInvoiceRepo>();
            A.CallTo(() => fakeRepo.AddInvoice(A<Invoice>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(false));

            var ep = Factory.Create<AddInvoiceEndpoint>(
                           A.Fake<ILogger<AddInvoiceEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            Assert.Equal("Error adding new invoice", response.Message);
            Assert.Null(response.Invoice);
        }

        [Fact]
        public async Task Invoice_Save_Returns_Exception()
        {
            AddInvoiceRequest invoiceRequest = new AddInvoiceRequest
            {
                SchemeType = "EU (2014 - 2020 Program)",
                PaymentType = "GBP",
                AccountType = "AP",
                SecondaryQuestion = "Any question"
            };

            var fakeRepo = A.Fake<IInvoiceRepo>();
            A.CallTo(() => fakeRepo.AddInvoice(A<Invoice>.Ignored, CancellationToken.None))
                    .Throws<NullReferenceException>();

            var ep = Factory.Create<AddInvoiceEndpoint>(
                           A.Fake<ILogger<AddInvoiceEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            Assert.Equal("Object reference not set to an instance of an object.", response.Message);
            Assert.Null(response.Invoice);
        }
    }
}
