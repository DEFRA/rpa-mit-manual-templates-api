
using InvoiceLinesAdd;
using Rpa.Mit.Manual.Templates.Api.Api.Tests.Integration.InvoiceLineTests;

namespace Rpa.Mit.Manual.Templates.Api.Core.Integration.Tests;

public class Tests(Sut sut) : TestBase<Sut>
{
    [Fact, Priority(0)]
    public async Task Valid_InvoiceLine_Input()
    {
        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            sut.AddInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.OK);
        res.Errors.Count.Should().Be(0);
    }

    [Fact, Priority(1)]
    public async Task Invalid_InvoiceLine_Input_Missing_Value()
    {
        sut.AddInvoiceLineRequest.Value = 0M;

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            sut.AddInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(2);
        res.Errors.Keys.Should().Equal("value", "fundCode");
    }

    [Fact, Priority(2)]
    public async Task Invalid_InvoiceLine_Input_Missing_Fundcode()
    {
        sut.AddInvoiceLineRequest.FundCode = string.Empty;

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            sut.AddInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(3);
        res.Errors.Keys.Should().Equal("invoiceRequestId", "value", "fundCode");
    }

    [Fact, Priority(3)]
    public async Task Invalid_InvoiceLine_Input_Missing_SchemeCode()
    {
        sut.AddInvoiceLineRequest.SchemeCode = string.Empty;

        var (rsp, res) = await sut.Client.POSTAsync<AddInvoiceIineEndpoint, AddInvoiceLineRequest, ErrorResponse>(
            sut.AddInvoiceLineRequest);

        rsp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        res.Errors.Count.Should().Be(2);
        res.Errors.Keys.Should().Equal("value", "schemeCode");
    }

    //[Fact, Priority(2)]
    //public async Task Valid_User_Input()
    //{
    //    var (rsp, res) = await App.Client.POSTAsync<AddInvoiceLineRequest, AddInvoiceLineResponse>(
    //                         new()
    //                         {
    //                             FirstName = "Mike",
    //                             LastName = "Kelso"
    //                         });

    //    rsp.StatusCode.Should().Be(HttpStatusCode.OK);
    //    res.Message.Should().Be("Hello Mike Kelso...");
    //}
}