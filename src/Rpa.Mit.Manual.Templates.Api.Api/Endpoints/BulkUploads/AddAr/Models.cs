using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace BulkUploads.AddAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadsArRequest
    {

        /// <summary>
        /// this is the initial org selection
        /// </summary>
        public string Org { get; set; } = string.Empty;

        public string SchemeType { get; set; } = string.Empty;

        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsArValidator : Validator<BulkUploadsArRequest>
        {
            public BulkUploadsArValidator()
            {
                RuleFor(x => x.Org)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Org must have a value");

                RuleFor(x => x.SchemeType)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("SchemeType must have a value");

                RuleFor(x => x.File)
                    .SetValidator(new FileValidator());
            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class Response
    {
        public BulkUploadArDataset? BulkUploadArDataset { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
