using System.Diagnostics.CodeAnalysis;


using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace InvoiceLines.Update
{
    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineRequest
    {

        public Guid Id { get; set; }

        public string InvoiceRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string Description { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBody { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    internal class UpdateInvoiceLineValidator : Validator<UpdateInvoiceLineRequest>
    {
        public UpdateInvoiceLineValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("InvoiceRequest Id is required!");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("Value must be greater than 0!");

            RuleFor(x => x.SchemeCode)
                .NotEmpty().WithMessage("SchemeCode is required!")
                .MinimumLength(3).WithMessage("SchemeCode is too short!");

            RuleFor(x => x.FundCode)
                .NotEmpty().WithMessage("FundCode is required!")
                .MinimumLength(3).WithMessage("FundCode is too short!")
                .MaximumLength(5).WithMessage("FundCode is too long!");

            RuleFor(x => x.MainAccount)
                .NotEmpty().WithMessage("MainAccount is required!");

            RuleFor(x => x.MarketingYear)
                .NotEmpty().WithMessage("MarketingYear is required!")
                .Must(x => int.TryParse(x, out _))
                .Length(4).WithMessage("MarketingYear requires 4 digits!");

            RuleFor(x => x.DeliveryBody)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("DeliveryBody is required!")
                .NotEmpty().WithMessage("DeliveryBody is required!");
        }
    }


    [ExcludeFromCodeCoverage]
    internal sealed class UpdateInvoiceLineResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceLine? InvoiceLine { get; set; }

        public decimal InvoiceRequestValue { get; set; }
    }
}
