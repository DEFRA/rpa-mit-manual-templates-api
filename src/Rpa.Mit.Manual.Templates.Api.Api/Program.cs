
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Rpa.Mit.Manual.Templates.Api.Api;
using Rpa.Mit.Manual.Templates.Api.Api.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces.Azure;

namespace Program
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        protected Program()
        { }

        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices.Configure(builder);

            builder.Services
               .AddFastEndpoints()
               .SwaggerDocument()

            .AddHostedService<PaymentHubBackgroundService<PaymentHubResponseRoot>>()
            .AddSingleton<IMessageHandler<PaymentHubResponseRoot>, PaymentHubResponseHandler>()

            .AddAzureBusComponents(builder);

            var tenantId = builder.Configuration["TENANT-ID"];
            var clientId = builder.Configuration["CLIENT-ID"];

            builder.Services
                   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(
                       o =>
                       {
                           o.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
                           o.TokenValidationParameters.ValidIssuer = $"https://sts.windows.net/{tenantId}/";
                           o.Audience = $"api://{clientId}";
                       }
                       );

            builder.Services.AddAuthorization();

            // add this to support excel reader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var app = builder.Build();

            app.UseAuthentication()
               .UseAuthorization()
               .UseFastEndpoints()
               .UseSwaggerGen();

            app.UseResponseCaching();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseHealthChecks("/healthy", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "ready"
            });
            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = check => check.Name == "live"
            });

            await app.RunAsync();
        }
    }
}