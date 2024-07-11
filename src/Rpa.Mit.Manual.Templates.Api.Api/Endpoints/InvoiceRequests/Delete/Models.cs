namespace DeleteInvoiceRequest
{
    internal sealed class DeleteInvoiceRequestRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        internal sealed class Validator : Validator<DeleteInvoiceRequestRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class DeleteInvoiceRequestResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
