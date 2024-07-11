using FakeItEasy;

using FastEndpoints;

using Invoices.Add;

using Microsoft.Extensions.Logging;

using InvoiceRequests.Add;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class InvoiceRequestTests
    {
        [Fact]
        public async Task InvoiceRequest_Save_Success()
        {
            InvoiceRequests.Add.AddInvoiceRequestRequest invoiceRequest = new AddInvoiceRequestRequest
            {
                FRN = "EU (2014 - 2020 Program)",
                SBI = "GBP",
                Description = "AP",
                DueDate = "EA",
                InvoiceRequestId = "Any question",
                AccountType = "QQ",
                Currency = "DDD",
                AgreementNumber = "1",
                MarketingYear ="2024",
                InvoiceId = Guid.NewGuid(),
                Vendor = "Q"
            };

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(true));

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            response.InvoiceRequest.Should().NotBeNull();
            response.InvoiceRequest?.AccountType.Should().Match("QQ");

            Assert.IsType<string>(response.InvoiceRequest?.InvoiceRequestId);
        }

        [Fact]
        public async Task InvoiceRequest_Save_Fail()
        {
            AddInvoiceRequestRequest invoiceRequest = new AddInvoiceRequestRequest
            {
                FRN = "EU (2014 - 2020 Program)",
                SBI = "GBP",
                Description = "AP",
                DueDate = "EA",
                InvoiceRequestId = "Any question",
                AccountType = "QQ",
                Currency = "DDD",
                AgreementNumber = "1",
                MarketingYear = "2024",
                InvoiceId = Guid.NewGuid(),
                Vendor = "Q"
            };

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(false));

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            Assert.Equal("Error adding new payment request", response.Message);
            Assert.Null(response.InvoiceRequest);
        }


        [Fact]
        public async Task InvoiceRequest_Save_Returns_Exception()
        {
            AddInvoiceRequestRequest invoiceRequest = new AddInvoiceRequestRequest
            {
                FRN = "EU (2014 - 2020 Program)",
                SBI = "GBP",
                Description = "AP",
                DueDate = "EA",
                InvoiceRequestId = "Any question",
                AccountType = "QQ",
                Currency = "DDD",
                AgreementNumber = "1",
                MarketingYear = "2024",
                InvoiceId = Guid.NewGuid(),
                Vendor = "Q"
            };

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Throws<NullReferenceException>();

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            Assert.Equal("Object reference not set to an instance of an object.", response.Message);
            Assert.Null(response.InvoiceRequest);
        }
    }
}
