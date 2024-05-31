using Asp.Versioning;
using FastEndpoints.Swagger;
using Rpa.Mit.Manual.Templates.Api.Api.Config;
using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Api.HealthChecks;

namespace Rpa.Mit.Manual.Templates.Api.Api
{
    public static class ConfigureServices
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();

            builder.Services
                   .AddFastEndpoints()
                   .SwaggerDocument();

            builder.Services.AddLogging();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddHealthChecks()
                   .AddCheck<LivenessCheck>("live")
                   .AddCheck<ReadinessCheck>("ready");

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
