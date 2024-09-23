using System.Diagnostics.CodeAnalysis;

namespace ApproveInvoiceAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceArRequest
    {
        /// <summary>
        /// invoice id
        /// </summary>
        public Guid Id { get; set; }

        internal sealed class Validator : Validator<ApproveInvoiceArRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class ApproveInvoiceArResponse
    {
        public bool Result { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
