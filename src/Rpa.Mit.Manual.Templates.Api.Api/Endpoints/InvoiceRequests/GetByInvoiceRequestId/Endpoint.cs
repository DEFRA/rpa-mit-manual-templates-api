﻿using System.Diagnostics.CodeAnalysis;

using InvoiceRequests.GetByInvoiceId;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetByInvoiceRequestId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceRequestIdEndpoint : Endpoint<GetByInvoiceRequestIdRequest, GetByInvoiceRequestIdResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<GetByInvoiceRequestIdEndpoint> _logger;

        public GetByInvoiceRequestIdEndpoint(
            ILogger<GetByInvoiceRequestIdEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Get("/invoicerequests/getbyid");
        }

        public override async Task HandleAsync(GetByInvoiceRequestIdRequest r, CancellationToken ct)
        {
            var response = new GetByInvoiceRequestIdResponse();

            try
            {
                response.InvoiceRequest = await _iInvoiceRequestRepo.GetInvoiceRequestByInvoiceRequestId(r.InvoiceRequestId, ct);

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}