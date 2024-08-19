
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

using Rpa.Mit.Manual.Templates.Api.Api;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

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

            builder.Services
               .AddFastEndpoints()
               .SwaggerDocument();

            // Add services to the container.
            ConfigureServices.Configure(builder);

            builder.Services.ConfigureAzure(builder.Configuration);

            builder.Services
                   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(
                       o =>
                       {
                           o.Authority = "https://login.microsoftonline.com/6f504113-6b64-43f2-ade9-242e05780007/v2.0";
                           o.TokenValidationParameters.ValidIssuer = "https://sts.windows.net/6f504113-6b64-43f2-ade9-242e05780007/";
                           o.Audience = "api://442bf74f-8332-4b81-9335-8d4d45b24eb6";
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

            app.UseAuthentication();

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