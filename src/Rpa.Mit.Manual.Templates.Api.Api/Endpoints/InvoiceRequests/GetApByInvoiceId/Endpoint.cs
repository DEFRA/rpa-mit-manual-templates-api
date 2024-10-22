﻿using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceRequestsAp.GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRequestsByInvoiceIdEndpoint : Endpoint<InvoiceRequestsGetByInvoiceIdRequest, InvoiceRequestsGetByInvoiceIdResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<InvoiceRequestsByInvoiceIdEndpoint> _logger;

        public InvoiceRequestsByInvoiceIdEndpoint(
            ILogger<InvoiceRequestsByInvoiceIdEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Get("/invoicerequests/getapbyinvoiceid");
        }

        public override async Task HandleAsync(InvoiceRequestsGetByInvoiceIdRequest r, CancellationToken ct)
        {
            var response = new InvoiceRequestsGetByInvoiceIdResponse();

            try
            {
                response.InvoiceRequests = await _iInvoiceRequestRepo.GetInvoiceRequestsByInvoiceId(r.InvoiceId, ct);

                await SendAsync(response, 200, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}