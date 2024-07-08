﻿using Invoices.Add;

using Microsoft.AspNetCore.Mvc.Formatters;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace DeleteInvoice
{
    internal sealed class DeleteInvoiceEndpoint : Endpoint<DeleteInvoiceRequest, DeleteInvoiceResponse>
    {
        private readonly IInvoiceRepo _iInvoiceDataRepo;
        private readonly ILogger<DeleteInvoiceEndpoint> _logger;

        public DeleteInvoiceEndpoint(
            ILogger<DeleteInvoiceEndpoint> logger,
            IInvoiceRepo iInvoiceDataRepo)
        {
            _logger = logger;
            _iInvoiceDataRepo = iInvoiceDataRepo;
        }

        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/invoices/delete");
        }

        public override async Task HandleAsync(DeleteInvoiceRequest r, CancellationToken ct)
        {
            DeleteInvoiceResponse response = new DeleteInvoiceResponse
            {
                Result = false
            };
            
            try
            {
                if (await _iInvoiceDataRepo.DeleteInvoice(r.InvoiceId, ct))
                {
                    response.Result = true;
                    response.Message = "Invoice deleted";
                }
                else
                {
                    response.Message = "Error deleting invoice";
                }

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