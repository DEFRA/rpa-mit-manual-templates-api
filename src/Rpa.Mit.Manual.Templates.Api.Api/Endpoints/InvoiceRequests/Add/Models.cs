using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceRequests.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequestRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        public Guid InvoiceId { get; set; }

        public string SourceSystem { get; set; } = "Manual";

        public string FRN { get; set; } = string.Empty;

        public int? SBI { get; set; } = null;

        public string Vendor { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Agreement Number is required")]
        public string AgreementNumber { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public string OriginalInvoiceNumber { get; set; } = string.Empty;

        public DateTime OriginalSettlementDate { get; set; } = default!;

        public DateTime RecoveryDate { get; set; } = default!;

        public string InvoiceCorrectionReference { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    internal class AddInvoiceRequestValidator : Validator<AddInvoiceRequestRequest>
    {
        public AddInvoiceRequestValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Matches("^GBP|EUR$")
                .WithMessage("The Currency must be either GBP or EUR");

            When(x => !string.IsNullOrEmpty(x.FRN), () => {
                RuleFor(x => x.FRN).NotEmpty()
                .Matches("^([0-9]{10})?$")
                .WithMessage("The FRN must be a 10-digit number or be empty.");
            });

            RuleFor(x => x.SBI).InclusiveBetween(105000000, 999999999)
                .WithMessage("The SBI is not in valid range (105000000 .. 999999999) or should be empty.");

            When(x => !string.IsNullOrEmpty(x.Vendor), () => {
                RuleFor(x => x.Vendor)
                 .Matches("^.{6}$|^$")
                 .WithMessage("The Vendor must be 6 characters or be empty.");
            });

            RuleFor(x => x.MarketingYear).NotEmpty()
                .Matches("^(201[5-9]|20[2-9]\\d|[2-9]\\d{3})$")
                .WithMessage("The Marketing Year must be after 2014");
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequestResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceRequest? InvoiceRequest { get; set; }
    }
}
