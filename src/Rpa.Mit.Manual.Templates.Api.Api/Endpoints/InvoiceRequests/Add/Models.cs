﻿using System.ComponentModel.DataAnnotations;
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

        public string SBI { get; set; } = string.Empty;

        [RegularExpression(@"^(201[5-9]|20[2-9]\d|[2-9]\d{3})$", ErrorMessage = "The Marketing Year must be after 2014")]
        public string MarketingYear { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "The Payment Request Number must be greater than 0")]
        public int InvoiceRequestNumber { get; set; }

        [Required(ErrorMessage = "The Agreement Number is required")]
        public string AgreementNumber { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        [RegularExpression("GBP|EUR", ErrorMessage = "The Currency must be either GBP or EUR")]
        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$", ErrorMessage = "The value must be valid number and have a maximum of 2 decimal places.")]
        public decimal Value { get; set; }

        public string AccountType { get; set; } = string.Empty;

        public string OriginalInvoiceNumber { get; set; } = string.Empty;

        public DateTime OriginalSettlementDate { get; set; } = default!;

        public DateTime RecoveryDate { get; set; } = default!;

        public string InvoiceCorrectionReference { get; set; } = string.Empty;

        [RegularExpression(@"^.{6}$|^$", ErrorMessage = "The Vendor must be 6 characters or be empty.")]
        public string Vendor { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    internal class AddInvoiceRequestValidator : Validator<AddInvoiceRequestRequest>
    {
        public AddInvoiceRequestValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("InvoiceId is required");

            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .Must(x => (!string.IsNullOrEmpty(x.FRN) ^ !string.IsNullOrEmpty(x.SBI)) ^ !string.IsNullOrEmpty(x.Vendor))
                .WithMessage("Select only one of FRN, SBI and Vendor");

            When(x => !string.IsNullOrEmpty(x.FRN), () => {
                RuleFor(x => x.FRN).NotEmpty()
                .Matches("^([0-9]{10})?$")
                .WithMessage("The FRN must be a 10-digit number or be empty.");
            });

            When(x => !string.IsNullOrEmpty(x.SBI), () => {
                RuleFor(x => x.SBI)
                .NotEmpty()
                .Matches("^(1050{5}|10[5-9]\\d{6}|1[1-9]\\d{7}|[2-9]\\d{8})?$")
                .WithMessage("The SBI is not in valid range (105000000 .. 999999999) or should be empty.");
            });

            When(x => !string.IsNullOrEmpty(x.Vendor), () => {
                RuleFor(x => x.Vendor)
                .NotEmpty()
                .WithMessage("Vendor must be longer than 3 characters.");
            });


        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequestResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceRequest? InvoiceRequest { get; set; }
    }
}
