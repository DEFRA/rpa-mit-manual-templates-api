using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadsApRequest
    {
        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsApValidator : Validator<BulkUploadsApRequest>
        {
            public BulkUploadsApValidator()
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
