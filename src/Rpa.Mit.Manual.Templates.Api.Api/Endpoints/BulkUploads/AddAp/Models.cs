using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadsRequest
    {
        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsValidator : Validator<BulkUploadsRequest>
        {
            public BulkUploadsValidator()
            {
                RuleFor(x => x.File)
                    .NotNull();
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
