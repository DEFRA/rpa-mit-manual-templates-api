﻿using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.PaymentRequests;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions;
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IInvoiceRepo, InvoiceRepo>();
        services.AddTransient<IPaymentRequestRepo, PaymentRequestRepo>();
        return services;
    }
}
