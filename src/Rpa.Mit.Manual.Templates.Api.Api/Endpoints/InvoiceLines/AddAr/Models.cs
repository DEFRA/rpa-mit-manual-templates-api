using System.Diagnostics.CodeAnalysis;

using InvoiceLines.AddAp;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace AddInvoiceIineAr
{
    /// <summary>
    /// Ar invoice line
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceLineArRequest : AddInvoiceLineRequest
    {
        /// <summary>
        /// calculated. either ‘ADMIN ERROR’ or ‘IRREGULARITY’
        /// </summary>
        public string DebtType { get; set; } = string.Empty;

        internal sealed class Validator : Validator<AddInvoiceLineArRequest>
        {
            public Validator()
            {
                RuleFor(x => x.DebtType)
                    .NotEmpty()
                    .Matches("^ADMIN ERROR|IRREGULARITY$")
                    .WithMessage("DebtType must be either ‘ADMIN ERROR’ or ‘IRREGULARITY’");
            }
        }
    }

    internal sealed class AddInvoiceLineArResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLineAr? InvoiceLine { get; set; }

        public decimal InvoiceRequestValue { get; set; }
    }
}
