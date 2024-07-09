global using FastEndpoints;

global using FluentValidation;

using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Rpa.Mit.Manual.Templates.Api.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddFastEndpoints()
   .SwaggerDocument(); //define a swagger document

// Add services to the container.
ConfigureServices.Configure(builder);

builder.Services.ConfigureAzure(builder.Configuration);

var app = builder.Build();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.UseAuthentication();

app.UseResponseCaching();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseOpenApi();
    //app.UseSwaggerUi(s => s.ConfigureDefaults());
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

public partial class Program 
{
    protected Program()
    {}
}