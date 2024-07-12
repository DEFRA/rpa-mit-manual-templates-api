using System.Diagnostics.CodeAnalysis;

namespace InvoiceLines.Delete
{
    [ExcludeFromCodeCoverage]
    public sealed class DeleteInvoiceLineRequest
    {
        public Guid InvoiceLineId { get; set; }

        internal sealed class Validator : Validator<DeleteInvoiceLineRequest>
        {
            public Validator()
            {
                RuleFor(x => x.InvoiceLineId)
                    .NotEmpty().WithMessage("InvoiceLineId is required!");
            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class DeleteInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public decimal InvoiceRequestValue { get; set; }
    }
}
