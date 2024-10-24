using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length)
                .NotNull()
                .LessThanOrEqualTo(1048576)
                .WithMessage("File size is larger than allowed - 1MB");

            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals("application/vnd.ms-excel.sheet.macroenabled.12"))
                .WithMessage("File type is wrong. Must be application/vnd.ms-excel with extension .xlsm");
        }
    }
}
