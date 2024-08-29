using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Rpa.Mit.Manual.Templates.Api.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Api.HealthChecks;

namespace Rpa.Mit.Manual.Templates.Api.Core.Integration.Tests;

public class App : AppFixture<Program.Program>
{
    protected override Task SetupAsync()
    {
        // place one-time setup for the fixture here
        return Task.CompletedTask;
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        // do host builder configuration here
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddApplicationServices();

        var descriptor = s.Single(s => s.ImplementationType == typeof(WorkerServiceBus));
        s.Remove(descriptor);

        s.AddMemoryCache();
        s
                .AddResponseCaching()
                .AddAuthorization();

        s.AddLogging();
        s.AddExceptionHandler<GlobalExceptionHandler>();
        s.AddProblemDetails();

        s.AddHealthChecks()
                .AddCheck<LivenessCheck>("live")
                .AddCheck<ReadinessCheck>("ready");
    }

    protected override Task TearDownAsync()
    {
        // do cleanups here
        return Task.CompletedTask;
    }
}