
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceLinesAdd
{
    [ExcludeFromCodeCoverage]
    public sealed class AddInvoiceLineRequest
    {
        public Guid Id { get; set; }

        public string InvoiceRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; }  = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBody { get; set; } = string.Empty;

    }

    public class Validator : Validator<AddInvoiceLineRequest>
    {
        public Validator()
        {
            RuleFor(x => x.InvoiceRequestId)
                .NotEmpty().WithMessage("InvoiceRequest Id is required!")
                .MinimumLength(10).WithMessage("InvoiceRequestId is too short!");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("Value must be greater than 0!");

            RuleFor(x => x.FundCode)
                .NotEmpty().WithMessage("FundCode is required!")
                .MinimumLength(3).WithMessage("FundCode is too short!")
                .MaximumLength(5).WithMessage("FundCode is too long!");

            RuleFor(x => x.SchemeCode)
                .NotEmpty().WithMessage("SchemeCode is required!")
                .MinimumLength(3).WithMessage("SchemeCode is too short!");

            RuleFor(x => x.MarketingYear)
                .NotEmpty().WithMessage("MarketingYear is required!")
                .Must(x => int.TryParse(x, out _))
                .Length(4).WithMessage("MarketingYear requires 4 digits!");
        }
    }

    [ExcludeFromCodeCoverage]
    public sealed class AddInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLine? InvoiceLine { get; set; }

        public decimal InvoiceRequestValue { get; set; }
    }
}
