using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Rpa.Mit.Manual.Templates.Api.Api.HealthChecks;
[ExcludeFromCodeCoverage]
public class ReadinessCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // CHECK FOR ANY DEPENDENCIES FOR READINESS
        return Task.FromResult(HealthCheckResult.Healthy("OK"));
    }
}