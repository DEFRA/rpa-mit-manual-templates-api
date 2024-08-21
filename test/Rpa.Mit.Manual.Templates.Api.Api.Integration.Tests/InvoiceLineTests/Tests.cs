
using FastEndpoints;

using InvoiceLines.Add;
using InvoiceLines.Delete;
using InvoiceLines.Update;

using Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests;

namespace Rpa.Mit.Manual.Templates.Api.Core.Integration.Tests;

public class Tests(Sut sut) : TestBase<Sut>
{
    [Fact]
    public async Task Valid_InvoiceLine_Input()
    {
        var addInvoiceLineRequest = new AddInvoiceLineRequest
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

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, AddInvoiceLineResponse>(
            addInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.OK);
        res.InvoiceRequestValue.Should().Be(12.12M);
        res.InvoiceLine?.Description.Should().Be("G00 - Gross value of claim");
        res.InvoiceLine?.FundCode.Should().Be("FUND1");
        res.InvoiceLine?.MainAccount.Should().Be("AR");
        res.InvoiceLine?.SchemeCode.Should().Be("code1");
        res.InvoiceLine?.MarketingYear.Should().Be("2020");
        res.InvoiceLine?.DeliveryBody.Should().Be("DB1");
        res.InvoiceLine?.InvoiceRequestId.Should().Be("333_YMALXCHG");
                       
        res.InvoiceLine?.Id.Should().NotBe("00000000-0000-0000-0000-000000000001");
    }

    [Fact]
    public async Task Invalid_InvoiceLine_Input_Missing_Fundcode()
    {
        var addInvoiceLineRequest = new AddInvoiceLineRequest
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

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            addInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Equal("fundCode");
    }

    [Fact]
    public async Task Invalid_InvoiceLine_Input_Missing_Value()
    {
        var addInvoiceLineRequest = new AddInvoiceLineRequest
        {
            Description = "G00 - Gross value of claim",
            FundCode = "FUND1",
            MainAccount = "AR",
            SchemeCode = "code1",
            MarketingYear = "2020",
            DeliveryBody = "DB1",
            InvoiceRequestId = "333_YMALXCHG"
        };

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            addInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Equal("value");
    }

    [Fact]
    public async Task Invalid_InvoiceLine_Input_Missing_SchemeCode()
    {
        var addInvoiceLineRequest = new AddInvoiceLineRequest
        {
            Value = 12.12M,
            Description = "G00 - Gross value of claim",
            FundCode = "FUND1",
            MainAccount = "AR",
            SchemeCode = string.Empty,
            MarketingYear = "2020",
            DeliveryBody = "DB1",
            InvoiceRequestId = "333_YMALXCHG"
        };

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            addInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Equal("schemeCode");
    }


    [Fact]
    public async Task Invalid_InvoiceLine_Input_Missing_InvoiceRequestId()
    {
        var addInvoiceLineRequest = new AddInvoiceLineRequest
        {
            Value = 12.12M,
            Description = "G00 - Gross value of claim",
            FundCode = "FUND1",
            MainAccount = "AR",
            SchemeCode = "code1",
            MarketingYear = "2020",
            DeliveryBody = "DB1",
            InvoiceRequestId = string.Empty
        };

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            addInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Equal("invoiceRequestId");
    }

    [Fact]
    public async Task InvoiceLine_Delete_Success()
    {
        var deleteInvoiceLineRequest = new DeleteInvoiceLineRequest
        {
            InvoiceLineId = Guid.NewGuid()
        };

        var (rsp, res) = await sut.Client.DELETEAsync<DeleteInvoiceLineEndpoint, DeleteInvoiceLineRequest, DeleteInvoiceLineResponse>(
            deleteInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.OK);
        res.InvoiceRequestValue.Should().Be(34.55M);
    }

    [Fact]
    public async Task InvoiceLine_Update_Success()
    {
        var updateInvoiceLineRequest = new UpdateInvoiceLineRequest
        {
            Id = Guid.NewGuid(),
            Value = 12.12M,
            Description = "G00 - Gross value of claim",
            FundCode = "FUND1",
            MainAccount = "AR",
            SchemeCode = "code1",
            MarketingYear = "2020",
            DeliveryBody = "DB1"
        };

        var (rsp, res) = await sut.Client.PUTAsync<UpdateInvoiceLineEndpoint, UpdateInvoiceLineRequest, UpdateInvoiceLineResponse>(
            updateInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.OK);
        res.InvoiceRequestValue.Should().Be(55.55M);
    }

    [Fact]
    public async Task InvoiceLine_Update_Fail_Missing_InvoiceLine_id()
    {
        var updateInvoiceLineRequest = new UpdateInvoiceLineRequest
        {
            Value = 12.12M,
            Description = "G00 - Gross value of claim",
            FundCode = "FUND1",
            MainAccount = "AR",
            SchemeCode = "code1",
            MarketingYear = "2020",
            DeliveryBody = "DB1"
        };

        var (rsp, res) = await sut.Client.PUTAsync<UpdateInvoiceLineEndpoint, UpdateInvoiceLineRequest, ErrorResponse>(
            updateInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(1);
        res.Errors.Keys.Should().Equal("id");
    }
}