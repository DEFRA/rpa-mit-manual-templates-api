using System.Diagnostics.CodeAnalysis;

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

        public string SchemeInvoiceTemplate { get; set; } = string.Empty;

        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsArValidator : Validator<BulkUploadsArRequest>
        {
            public BulkUploadsArValidator()
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
        public BulkUploadArDataset? BulkUploadArDataset { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
