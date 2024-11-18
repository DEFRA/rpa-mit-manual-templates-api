using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.Extensions;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadsApRequest
    {
        public required IFormFile File { get; set; }

        /// <summary>
        /// this is the initial org selection
        /// </summary>
        public required string Org { get; set; }

        public string SchemeType { get; set; } = string.Empty;

        internal sealed class BulkUploadsApValidator : Validator<BulkUploadsApRequest>
        {
            public BulkUploadsApValidator()
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
        public BulkUploadApDataset? BulkUploadApDataset { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
