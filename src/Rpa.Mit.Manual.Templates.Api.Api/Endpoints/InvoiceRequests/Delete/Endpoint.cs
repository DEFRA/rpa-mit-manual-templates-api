using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace InvoiceRequests.Delete
{
    internal sealed class DeleteInvoiceRequestEndpoint : Endpoint<DeleteInvoiceRequestRequest, DeleteInvoiceRequestResponse>
    {
        private readonly IInvoiceRequestRepo _iInvoiceRequestRepo;
        private readonly ILogger<DeleteInvoiceRequestEndpoint> _logger;

        public DeleteInvoiceRequestEndpoint(
            ILogger<DeleteInvoiceRequestEndpoint> logger,
            IInvoiceRequestRepo iInvoiceRequestRepo)
        {
            _logger = logger;
            _iInvoiceRequestRepo = iInvoiceRequestRepo;
        }

        public override void Configure()
        {
            Delete("invoicerequests/delete");
        }

        public override async Task HandleAsync(DeleteInvoiceRequestRequest r, CancellationToken ct)
        {
            DeleteInvoiceRequestResponse response = new DeleteInvoiceRequestResponse
            {
                Result = false
            };

            try
            {
                if (await _iInvoiceRequestRepo.DeleteInvoiceRequest(r.InvoiceRequestId, ct))
                {
                    response.Result = true;
                    response.Message = "Invoice request deleted";
                }
                else
                {
                    response.Message = "Error deleting invoice request";
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