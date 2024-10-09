using InvoiceLines.Update;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace UpdateInvoiceLineAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineArRequest : UpdateInvoiceLineRequest
    {
        public string DebtType { get; set; } = string.Empty;

        internal sealed class Validator : Validator<UpdateInvoiceLineArRequest>
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

    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineArResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLineAr? InvoiceLine { get; set; }

        public decimal InvoiceRequestValue { get; set; }
    }
}
