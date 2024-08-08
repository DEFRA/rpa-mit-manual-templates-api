using System.Diagnostics.CodeAnalysis;

namespace ApproveInvoice
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceRequest
    {
        /// <summary>
        /// invoice id
        /// </summary>
        public Guid Id { get; set; }

        internal sealed class Validator : Validator<ApproveInvoiceRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceResponse
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
