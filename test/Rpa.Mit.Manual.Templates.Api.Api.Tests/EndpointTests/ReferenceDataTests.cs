using Xunit.Priority;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests;

public class ReferenceDataTests// : TestBase<App>
{
    private App? App { get; }

    public ReferenceDataTests()
    {
        App = new();
    }

    [Fact]
    public async Task CanGetReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/referencedata/get");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CanGetOrganisationReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/organisations/get");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CanGetPaymentTypesReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/paymenttypes/get");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CanGetSchemeTypesReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/schemetypes/get");

        // Assert
        Assert.NotNull(result);
    }
}
