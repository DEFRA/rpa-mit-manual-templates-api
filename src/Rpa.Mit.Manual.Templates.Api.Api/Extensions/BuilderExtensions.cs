using Rpa.Mit.Manual.Templates.Api.Api.Config;

using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;

using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder, AppInsightsConfig appInsightsConfig)
    {
        if (!string.IsNullOrEmpty(appInsightsConfig.ConnectionString))
        {
            builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConfig.ConnectionString;
                if (builder.Environment.IsProduction())
                {
                    options.Credential = new DefaultAzureCredential();
                }
            });
            if (!string.IsNullOrEmpty(appInsightsConfig.CloudRole))
            {
                var resourceAttributes = new Dictionary<string, object> { { "service.name", appInsightsConfig.CloudRole } };
                builder.Services.ConfigureOpenTelemetryTracerProvider((sp, b) => b.ConfigureResource(resourceBuilder => resourceBuilder.AddAttributes(resourceAttributes)));
            }
            Console.WriteLine("App Insights Running!");
        }
        else
        {
            Console.WriteLine("App Insights Not Running!");
        }
        return builder;
    }
}