using FakeItEasy;

using FastEndpoints;
using FastEndpoints.Testing;

using FluentAssertions.Execution;

using GetPaymentTypes;

using GetSchemeTypes;

using Microsoft.Extensions.Logging;

using OpenTelemetry.Trace;

using Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

using Xunit.Sdk;

namespace Rpa.Mit.Manual.Templates.Api.Api.Tests.EndpointTests;

public class ReferenceDataTests// : TestBase<App>
{
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

        response.ReferenceData?.PaymentTypes.Should().NotBeNull();
        response.ReferenceData?.PaymentTypes.Should().AllBeOfType<PaymentType>();
        response.ReferenceData?.PaymentTypes.Should().HaveCount(0);

        response.ReferenceData?.Organisations.Should().NotBeNull();
        response.ReferenceData?.Organisations.Should().AllBeOfType<Organisation>();
        response.ReferenceData?.Organisations.Should().HaveCount(0);

        response.ReferenceData?.SchemeCodes.Should().NotBeNull();
        response.ReferenceData?.SchemeCodes.Should().HaveCount(0);
    }

    [Fact]
    public async Task CanGetPaymentTypesReferenceDataEndpoint()
    {
        List<PaymentType> paymentTypes =
        [
            new PaymentType{ Code = "code1", Description="qwer" },
            new PaymentType{ Code = "code2", Description="asdf"  },
            new PaymentType{ Code = "code3", Description="zxcv" }
        ];

        var fakeRepo = A.Fake<IReferenceDataRepo>();
        A.CallTo(() => fakeRepo.GetCurrencyReferenceData(CancellationToken.None))
                .Returns(Task.FromResult(paymentTypes.AsEnumerable()));

        var ep = Factory.Create<GetPaymentTypesEndpoint>(
                       A.Fake<ILogger<GetPaymentTypesEndpoint>>(),
                       fakeRepo);

        await ep.HandleAsync(default);
        var response = ep.Response;

        response.Should().NotBeNull();
        response.PaymentTypes.Should().HaveCount(3);
    }

    [Fact]
    public async Task CanGetSchemeTypesReferenceDataEndpoint()
    {
        List<SchemeType> paymentTypes =
        [
            new SchemeType{ Code = "code1", Description="qwer" },
            new SchemeType{ Code = "code2", Description="asdf"  },
            new SchemeType{ Code = "code3", Description="zxcv" }
        ];

        var fakeRepo = A.Fake<IReferenceDataRepo>();
        A.CallTo(() => fakeRepo.GetSchemeTypeReferenceData(CancellationToken.None))
                .Returns(Task.FromResult(paymentTypes.AsEnumerable()));

        var ep = Factory.Create<GetSchemeTypesEndpoint>(
                       A.Fake<ILogger<GetSchemeTypesEndpoint>>(),
                       fakeRepo);

        await ep.HandleAsync(default);
        var response = ep.Response;

        response.Should().NotBeNull();
        response.SchemeTypes.Should().HaveCount(3);
    }
}
