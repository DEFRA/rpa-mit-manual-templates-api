using FakeItEasy;

using FastEndpoints;
using FastEndpoints.Testing;

using FluentAssertions.Execution;

using Microsoft.Extensions.Logging;

using OpenTelemetry.Trace;

using Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using Xunit.Sdk;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests;

public class ReferenceDataTests// : TestBase<App>
{
    private App? App { get; }

    public ReferenceDataTests()
    {
        App = new();
    }

    [Fact]
    public async Task ReferenceData_GetAll()
    {
        ReferenceData referenceData = new ReferenceData();

        var fakeRepo = A.Fake<IReferenceDataRepo>();
        A.CallTo(() => fakeRepo.GetAllReferenceData(CancellationToken.None))
                .Returns(Task.FromResult(referenceData));

        var ep = Factory.Create<GetReferenceDataEndpoint>(
                       A.Fake<ILogger<GetReferenceDataEndpoint>>(),
                       fakeRepo);

        await ep.HandleAsync(default);
        var response = ep.Response;

        response.Should().NotBeNull();

        response.ReferenceData.Should().NotBeNull();

        response.ReferenceData.PaymentTypes.Should().NotBeNull();
        response.ReferenceData.PaymentTypes.Should().AllBeOfType<PaymentType>();
        response.ReferenceData.PaymentTypes.Should().HaveCount(0);

        response.ReferenceData.Organisations.Should().NotBeNull();
        response.ReferenceData.Organisations.Should().AllBeOfType<Organisation>();
        response.ReferenceData.Organisations.Should().HaveCount(0);

        response.ReferenceData.SchemeCodes.Should().NotBeNull();
        response.ReferenceData.SchemeCodes.Should().HaveCount(0);
    }

    [Fact]
    public async Task CanGetPaymentTypesReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/paymenttypes/get");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("OK", result.StatusCode.ToString());
    }

    [Fact]
    public async Task CanGetSchemeTypesReferenceDataEndpoint()
    {
        // Act
        var client = App!.CreateClient();

        var result = await client.GetAsync("/schemetypes/get");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("OK", result.StatusCode.ToString());
    }
}
