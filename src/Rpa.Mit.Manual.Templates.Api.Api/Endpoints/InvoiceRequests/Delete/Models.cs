namespace DeleteInvoiceRequest
{
    internal sealed class DeleteInvoiceRequestRequest
    {
        public string Message { get; set; } = string.Empty;

        internal sealed class Validator : Validator<DeleteInvoiceRequestRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class DeleteInvoiceRequestResponse
    {
        public string Message => "This endpoint hasn't been implemented yet!";
    }
}
