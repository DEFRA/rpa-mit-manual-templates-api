using FakeItEasy;

using FastEndpoints;

using InvoiceLines.Add;

using Microsoft.Extensions.Logging;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests
{
    public class ImvoiceLineTests
    {
        [Fact]
        public async Task InvoiceLine_Save_Success()
        {
            AddInvoiceLineRequest invoiceRequest = new AddInvoiceLineRequest
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

            var fakeRepo = A.Fake<IInvoiceLineRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceLine(A<InvoiceLine>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(12.12M));

            var ep = Factory.Create<AddInvoiceIineEndpoint>(
                           A.Fake<ILogger<AddInvoiceIineEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            response.InvoiceLine.Should().NotBeNull();
            response.InvoiceRequestValue.Should().Be(12.12M);

            Assert.IsType<InvoiceLine>(response.InvoiceLine);
        }


        [Fact]
        public async Task Invalid_InvoiceLine_Input_Missing_Fundcode()
        {
            var invoiceLineRequest = new AddInvoiceLineRequest
            {
                Value = 12.12M,
                Description = "G00 - Gross value of claim",
                FundCode = string.Empty,
                MainAccount = "AR",
                SchemeCode = "code1",
                MarketingYear = "2020",
                DeliveryBody = "DB1",
                InvoiceRequestId = "333_YMALXCHG"
            };


            var fakeRepo = A.Fake<IInvoiceLineRepo>();
            A.CallTo(() => fakeRepo.AddInvoiceLine(A<InvoiceLine>.Ignored, CancellationToken.None))
                    .Returns(Task.FromResult(12.12M));

            var ep = Factory.Create<AddInvoiceIineEndpoint>(
                           A.Fake<ILogger<AddInvoiceIineEndpoint>>(),
                           fakeRepo);

            await ep.HandleAsync(invoiceLineRequest, default);
            var response = ep.Response;

            response.Should().NotBeNull();
            //var res = await ep.ExecuteAsync(invoiceLineRequest, CancellationToken.None);
            //res.Should().BeOfType<AddInvoiceLineResponse>();

            //res.Message.Should().Be(string.Empty);
        }
    }
}
