global using FastEndpoints;
using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Rpa.Mit.Manual.Templates.Api.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices.Configure(builder);

var app = builder.Build();

app.UseAuthentication();

app.UseFastEndpoints()
    .UseSwaggerGen();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // app.UseOpenApi();
    app.UseSwaggerUi(s => s.ConfigureDefaults());
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

app.Run();

public partial class Program { }