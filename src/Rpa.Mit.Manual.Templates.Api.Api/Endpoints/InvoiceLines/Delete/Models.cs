namespace DeleteInvoiceLine
{
    internal sealed class DeleteInvoiceLineRequest
    {
        public Guid InvoiceLineId { get; set; }

        internal sealed class Validator : Validator<DeleteInvoiceLineRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class DeleteInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public decimal InvoiceRequestValue { get; set; }
    }
}
