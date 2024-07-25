
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Rpa.Mit.Manual.Templates.Api.Api;

namespace GetSchemeTypes
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

            // add this to support excel reader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var app = builder.Build();

            app.UseFastEndpoints()
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