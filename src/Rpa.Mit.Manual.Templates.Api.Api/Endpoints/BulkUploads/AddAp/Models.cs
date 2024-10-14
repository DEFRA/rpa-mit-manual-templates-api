using System.Diagnostics.CodeAnalysis;

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
        public string Org { get; set; } = string.Empty;

        public string SchemeInvoiceTemplate { get; set; } = string.Empty;

        internal sealed class BulkUploadsApValidator : Validator<BulkUploadsApRequest>
        {
            public BulkUploadsApValidator()
            {
                RuleFor(x => x.Org)
                    .NotNull().NotEmpty()
                                        .WithMessage("Org must have a value");

                RuleFor(x => x.SchemeInvoiceTemplate)
                    .NotNull().NotEmpty()
                                        .WithMessage("SchemeInvoiceTemplate must have a value");

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
