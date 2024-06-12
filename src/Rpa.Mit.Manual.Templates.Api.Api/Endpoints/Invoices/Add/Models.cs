
using System.Diagnostics.CodeAnalysis;


using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Enums;

namespace Invoices.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequest
    {
        public Guid Id { get; set; }

        public string PaymentType { get; set; } = default!;

        public string AccountType { get; set; } = default!;

        public string DeliveryBody { get; set; } = default!;

        public string SecondaryQuestion { get; set; } = default!;

        public string SchemeType { get; set; } = default!;

        public decimal Value { get; set; } = 0.0M;

        public string Status { get; private set; } = InvoiceStatuses.New;

        public string Reference { get; set; } = default!;


        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsGBP => PaymentRequests.Where(x => x.Currency == "GBP").Sum(x => x.Value);
        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsEUR => PaymentRequests.Where(x => x.Currency == "EUR").Sum(x => x.Value);
        //[JsonIgnore]
        //public bool CanBeSentForApproval => Status == InvoiceStatuses.New && PaymentRequests.All(x => x.Value != 0 && x.InvoiceLines.Any());

        //public string ApprovalGroup
        //{
        //    get
        //    {
        //        if (DeliveryBody == "RPA")
        //        {
        //            return SchemeType;
        //        }
        //        return DeliveryBody;
        //    }
        //}
    }

    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceValidator : Validator<AddInvoiceRequest>
    {
        public InvoiceValidator()
        {
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
