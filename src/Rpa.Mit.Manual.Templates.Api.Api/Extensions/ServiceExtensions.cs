﻿using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Rpa.Mit.Manual.Templates.Api.ReferenceDataEndPoint;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads;
using System.Diagnostics.CodeAnalysis;
using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions;
[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IApprovalsRepo, ApprovalsRepo>();
        services.AddTransient<IBulkUploadRepo, BulkUploadRepo>();
        services.AddTransient<IInvoiceLineRepo, InvoiceLineRepo>();
        services.AddTransient<IReferenceDataRepo, ReferenceDataRepo>();
        services.AddTransient<IInvoiceRepo, InvoiceRepo>();
        services.AddTransient<IInvoiceRequestRepo, InvoiceRequestRepo>();

        services.AddTransient<IApImporterService, ApImporterService>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
