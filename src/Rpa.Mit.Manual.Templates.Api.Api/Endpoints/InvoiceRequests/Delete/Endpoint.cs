namespace DeleteInvoiceRequest
{
    internal sealed class Endpoint : Endpoint<DeleteInvoiceRequestRequest, DeleteInvoiceRequestResponse>
    {
        public override void Configure()
        {
            Post("invoicerequests/delete");
        }

        public override async Task HandleAsync(DeleteInvoiceRequestRequest r, CancellationToken c)
        {
            await SendAsync(new DeleteInvoiceRequestResponse());
        }
    }
}