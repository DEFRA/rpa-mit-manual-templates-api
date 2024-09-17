﻿using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceLines.Delete
{
    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceLineEndpoint : Endpoint<DeleteInvoiceLineRequest, DeleteInvoiceLineResponse>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<DeleteInvoiceLineEndpoint> _logger;

        public DeleteInvoiceLineEndpoint(
            ILogger<DeleteInvoiceLineEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Delete("/invoicelines/delete");
        }

        public override async Task HandleAsync(DeleteInvoiceLineRequest r, CancellationToken ct)
        {
            DeleteInvoiceLineResponse response = new();

            try
            {
                var res = await _iInvoiceLineRepo.DeleteInvoiceLine(r.InvoiceLineId, ct);

                response.InvoiceRequestValue = res;

                await SendAsync(response, cancellation: ct);
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