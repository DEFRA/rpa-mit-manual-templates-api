using FakeItEasy;

using FastEndpoints;

using Invoices.Add;

using Microsoft.Extensions.Logging;

using PaymentsRequests.Add;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class PaymentRequestTests
    {
        [Fact]
        public async Task PaymentRequest_Save_Success()
        {
            AddPaymentRequest paymentRequest = new AddPaymentRequest
            {
                FRN = "EU (2014 - 2020 Program)",
                SBI = "GBP",
                Description = "AP",
                DueDate = "EA",
                PaymentRequestId = "Any question",
                AccountType = "QQ",
                Currency = "DDD",
                AgreementNumber = "1",
                MarketingYear ="2024",
                InvoiceId = Guid.NewGuid(),
                Vendor = "Q"
            };

            var fakeRepo = A.Fake<IPaymentRequestRepo>();
            A.CallTo(() => fakeRepo.AddPaymentRequest(A<PaymentRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(true));

            var ep = Factory.Create<AddPaymentsRequestEndpoint>(
                           A.Fake<ILogger<AddPaymentsRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(paymentRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            response.PaymentRequest.Should().NotBeNull();
            response.PaymentRequest?.AccountType.Should().Match("QQ");

            Assert.IsType<string>(response.PaymentRequest?.PaymentRequestId);
        }

        [Fact]
        public async Task PaymentRequest_Save_Fail()
        {
            AddPaymentRequest paymentRequest = new AddPaymentRequest
            {
                FRN = "EU (2014 - 2020 Program)",
                SBI = "GBP",
                Description = "AP",
                DueDate = "EA",
                PaymentRequestId = "Any question",
                AccountType = "QQ",
                Currency = "DDD",
                AgreementNumber = "1",
                MarketingYear = "2024",
                InvoiceId = Guid.NewGuid(),
                Vendor = "Q"
            };

            var fakeRepo = A.Fake<IPaymentRequestRepo>();
            A.CallTo(() => fakeRepo.AddPaymentRequest(A<PaymentRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(false));

            var ep = Factory.Create<AddPaymentsRequestEndpoint>(
                           A.Fake<ILogger<AddPaymentsRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(paymentRequest, default);
            var response = ep.Response;

            Assert.Equal("Error adding new payment request", response.Message);
            Assert.Null(response.PaymentRequest);
        }
    }
}
