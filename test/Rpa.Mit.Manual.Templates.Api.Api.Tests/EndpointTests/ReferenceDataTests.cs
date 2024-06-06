using System.Net;

using Azure;

using FastEndpoints;
using FastEndpoints.Testing;

using Xunit.Abstractions;
using Xunit.Priority;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests;

public class ReferenceDataTests// : TestBase<App>
{
    private readonly ITestOutputHelper? _output;
    private TestTimeProvider Clock { get; } = new();
    private App? App { get; }

    public ReferenceDataTests(ITestOutputHelper output)
    {
        _output = output;
        App = new();
    }

    [Fact, Priority(1)]
    public async Task CanGetReferenceDataEndpoint()
    {
        // Arrange
        Clock.SetTime(0, 0);
        // Act
        var client = App.CreateClient();

        var result = await client.GetAsync("/referencedata/get");

        // Assert
        Assert.NotNull(result);
    }
}
