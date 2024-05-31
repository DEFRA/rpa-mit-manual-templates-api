global using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Rpa.Mit.Manual.Templates.Api;
using Rpa.Mit.Manual.Templates.Api.Api;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
ConfigureServices.Configure(builder);

builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));


var app = builder.Build();

app.UseFastEndpoints();
  //.UseSwaggerGen();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // app.UseOpenApi();
    app.UseSwaggerUi(s => s.ConfigureDefaults());
}
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseHealthChecks("/healthy", new HealthCheckOptions()
{
    Predicate = check => check.Name == "ready"
});
app.UseHealthChecks("/healthz", new HealthCheckOptions()
{
    Predicate = check => check.Name == "live"
});

app.Run();