using System.Diagnostics.CodeAnalysis;

using Asp.Versioning;

using Rpa.Mit.Manual.Templates.Api.Api.Config;
using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Api.HealthChecks;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    [ExcludeFromCodeCoverage]
    public static class ConfigureServices
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddMemoryCache();
            builder.Services
                   .AddResponseCaching()
                   .AddAuthorization();

            builder.Services.AddLogging();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddHealthChecks()
                   .AddCheck<LivenessCheck>("live")
                   .AddCheck<ReadinessCheck>("ready");

            builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("AzureAd"));
            builder.Services.Configure<GovNotify>(builder.Configuration.GetSection("GOVNOTIFY"));
            builder.Services.Configure<PaymentHub>(builder.Configuration.GetSection("PAYMENTHUB"));
            builder.Services.Configure<PostGres>(builder.Configuration.GetSection("POSTGRES"));
            builder.Services.Configure<PaymentHub>(builder.Configuration.GetSection("PAYMENTHUB"));

            var appInsightsConfig = new AppInsightsConfig
            {
                CloudRole = builder.Configuration.GetValue<string>("APPINSIGHTS_CLOUDROLE") ?? "",
                ConnectionString = builder.Configuration.GetValue<string>("APPINSIGHTS_CONNECTIONSTRING") ?? ""
            };

            builder.ConfigureOpenTelemetry(appInsightsConfig);

            builder.Services.AddAuthentication();

            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
    }
}
