
using System.Diagnostics.CodeAnalysis;


using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Enums;

namespace Invoices.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequest
    {
        public string PaymentType { get; set; } = default!;

        public string AccountType { get; set; } = default!;

        public string DeliveryBody { get; set; } = default!;

        public string SecondaryQuestion { get; set; } = default!;

        public string SchemeType { get; set; } = default!;

        public decimal Value { get; set; } = 0.0M;

        public string Status { get; private set; } = InvoiceStatuses.New;

        public string Reference { get; set; } = default!;
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceValidator : Validator<AddInvoiceRequest>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.PaymentType)
                .NotNull()
                     .WithMessage("PaymentType is required")
                .NotEmpty()
                    .WithMessage("PaymentType is required");

            RuleFor(x => x.AccountType)
                .NotNull()
                     .WithMessage("AccountTypeis required")
                .NotEmpty()
                    .WithMessage("AccountType is required");

            RuleFor(x => x.DeliveryBody)
                .NotNull()
                     .WithMessage("Delivery Body is required")
                .NotEmpty()
                    .WithMessage("Delivery Body is required");
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceResponse
    {
        public string Message { get; set; } = string.Empty;

        public Invoice? Invoice { get; set; }
    }
}
