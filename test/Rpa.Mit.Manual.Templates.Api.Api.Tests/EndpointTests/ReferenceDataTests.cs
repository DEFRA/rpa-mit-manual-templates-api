using FakeItEasy;

using FastEndpoints;
using FastEndpoints.Testing;

using FluentAssertions.Execution;

using Microsoft.Extensions.Logging;

using Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

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

        using (new AssertionScope())
        {
            response.Should().NotBeNull();

            response.ReferenceData.Should().NotBeNull();

            response.ReferenceData?.PaymentTypes.Should().NotBeNull();
            response.ReferenceData?.PaymentTypes.Should().AllBeOfType<PaymentType>();
                              
            response.ReferenceData?.Organisations.Should().NotBeNull();
            response.ReferenceData?.Organisations.Should().AllBeOfType<Organisation>();
                              
            response.ReferenceData?.SchemeCodes.Should().NotBeNull();
            response.ReferenceData?.SchemeCodes.Should().AllBeOfType<SchemeCode>();
                              
            response.ReferenceData?.PaymentTypes.Should().NotBeNull();
            response.ReferenceData?.PaymentTypes.Should().AllBeOfType<PaymentType>();
        }
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
