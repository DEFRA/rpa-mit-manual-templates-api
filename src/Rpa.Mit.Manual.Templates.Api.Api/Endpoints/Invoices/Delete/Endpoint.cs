namespace DeleteInvoice
{
    internal sealed class DeleteInvoiceEndpoint : Endpoint<DeleteInvoiceRequest, DeleteInvoiceResponse>
    {
        public override void Configure()
        {
            // temp allow anon
            AllowAnonymous();
            Post("/invoices/delete");
        }

        public override async Task HandleAsync(DeleteInvoiceRequest r, CancellationToken c)
        {
            await SendAsync(new DeleteInvoiceResponse());
        }
    }
}