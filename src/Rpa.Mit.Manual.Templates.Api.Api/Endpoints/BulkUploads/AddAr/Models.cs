using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace BulkUploads.AddAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadsArRequest
    {
        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsArValidator : Validator<BulkUploadsArRequest>
        {
            public BulkUploadsArValidator()
            {
                RuleFor(x => x.File)
                    .NotNull();
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
