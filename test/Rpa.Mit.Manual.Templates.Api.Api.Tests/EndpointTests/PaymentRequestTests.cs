﻿using FakeItEasy;

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
            PaymentsRequests.Add.AddInvoiceRequest paymentRequest = new PaymentsRequests.Add.AddInvoiceRequest
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

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(true));

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(paymentRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            response.PaymentRequest.Should().NotBeNull();
            response.PaymentRequest?.AccountType.Should().Match("QQ");

            Assert.IsType<string>(response.PaymentRequest?.InvoiceRequestId);
        }

        [Fact]
        public async Task PaymentRequest_Save_Fail()
        {
            PaymentsRequests.Add.AddInvoiceRequest paymentRequest = new PaymentsRequests.Add.AddInvoiceRequest
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

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(false));

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(paymentRequest, default);
            var response = ep.Response;

            Assert.Equal("Error adding new payment request", response.Message);
            Assert.Null(response.PaymentRequest);
        }


        [Fact]
        public async Task PaymentRequest_Save_Returns_Exception()
        {
            PaymentsRequests.Add.AddInvoiceRequest paymentRequest = new PaymentsRequests.Add.AddInvoiceRequest
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

            var fakeRepo = A.Fake<IInvoiceRequestRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceRequest(A<InvoiceRequest>.Ignored, CancellationToken.None))
                    .Throws<NullReferenceException>();

            var ep = Factory.Create<AddInvoiceRequestEndpoint>(
                           A.Fake<ILogger<AddInvoiceRequestEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(paymentRequest, default);
            var response = ep.Response;

            Assert.Equal("Object reference not set to an instance of an object.", response.Message);
            Assert.Null(response.PaymentRequest);
        }
    }
}
