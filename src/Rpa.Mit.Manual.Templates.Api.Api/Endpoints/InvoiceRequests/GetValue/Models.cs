namespace GetInvoiceRequest
{
    internal sealed class GetInvoiceRequestValueRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        internal sealed class Validator : Validator<GetInvoiceRequestValueRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class GetInvoiceRequestValueResponse
    {
        public decimal InvoiceRequestValue { get; set; } = 0.0M;

        public string Message { get; set; } = string.Empty;
    }
}
