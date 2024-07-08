using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.PaymentRequests;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IInvoiceLineRepo, InvoiceLineRepo>();
        services.AddTransient<IReferenceDataRepo, ReferenceDataRepo>();
        services.AddTransient<IInvoiceRepo, InvoiceRepo>();
        services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();
        return services;
    }
}
