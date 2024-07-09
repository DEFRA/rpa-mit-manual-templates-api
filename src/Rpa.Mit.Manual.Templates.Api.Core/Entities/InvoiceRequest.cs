using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRequest
    {
        public string InvoiceRequestId { get; set; } = string.Empty;

        public Guid InvoiceId { get; set; }

        public string SourceSystem { get; set; } = "Manual";

        [DisplayName("FRN")]
        [RegularExpression(@"^([0-9]{10})?$", ErrorMessage = "The FRN must be a 10-digit number or be empty.")]
        public string FRN { get; set; } = string.Empty;

        [DisplayName("SBI")]
        [RegularExpression(@"^(1050{5}|10[5-9]\d{6}|1[1-9]\d{7}|[2-9]\d{8})?$", ErrorMessage = "The SBI is not in valid range (105000000 .. 999999999) or should be empty.")]
        public string SBI { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Marketing Year must be after 2014")]
        [RegularExpression(@"^(201[5-9]|20[2-9]\d|[2-9]\d{3})$", ErrorMessage = "The Marketing Year must be after 2014")]
        public string MarketingYear { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Payment Request Number must be greater than 0")]
        public int InvoiceRequestNumber { get; set; }

        [Required(ErrorMessage = "The Agreement Number is required")]
        public string AgreementNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression("GBP|EUR", ErrorMessage = "The Currency must be either GBP or EUR")]
        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2})?$", ErrorMessage = "The value must be valid number and have a maximum of 2 decimal places.")]
        public decimal Value { get; set; }

        public IEnumerable<InvoiceLine> InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLine>();

        public string AccountType { get; set; } = string.Empty;

        //[RequiredIfAR]
        [DisplayName("Original Claim Reference")]
        public string OriginalInvoiceNumber { get; set; } = string.Empty;

        //[RequiredIfAR]
        [DisplayName("Original AP Invoice Settlement Date")]
        public DateTime OriginalSettlementDate { get; set; } = default!;

        //[RequiredIfAR]
        [DisplayName("Earliest date possible recovery first identified")]
        public DateTime RecoveryDate { get; set; } = default!;

        [DisplayName("Correction Reference - Previous AR Invoice ID")]
        public string InvoiceCorrectionReference { get; set; } = string.Empty;

        [DisplayName("Vendor")]
        [RegularExpression(@"^.{6}$|^$", ErrorMessage = "The Vendor must be 6 characters or be empty.")]
        public string Vendor { get; set; } = string.Empty;

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;


        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;
    }
}
