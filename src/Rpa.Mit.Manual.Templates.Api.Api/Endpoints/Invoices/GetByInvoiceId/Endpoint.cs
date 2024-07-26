﻿using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetByInvoiceId
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetByInvoiceIdEndpoint : Endpoint<GetByInvoiceIdRequest, GetByInvoiceIdResponse>
    {
        private readonly IInvoiceRepo _iInvoiceRepo;
        private readonly ILogger<GetByInvoiceIdEndpoint> _logger;

        public GetByInvoiceIdEndpoint(
            ILogger<GetByInvoiceIdEndpoint> logger,
            IInvoiceRepo iInvoiceRepo)
        {
            _logger = logger;
            _iInvoiceRepo = iInvoiceRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Get("/invoices/getbyid");
        }

        public override async Task HandleAsync(GetByInvoiceIdRequest r, CancellationToken ct)
        {
            GetByInvoiceIdResponse response = new GetByInvoiceIdResponse();

            try
            {
                response.Invoice = await _iInvoiceRepo.GetInvoiceByInvoiceId(r.InvoiceId, ct);

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