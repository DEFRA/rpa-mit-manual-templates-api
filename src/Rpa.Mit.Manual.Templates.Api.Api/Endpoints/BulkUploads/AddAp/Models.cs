namespace BulkUploads.AddAp
{
    internal sealed class BulkUploadsRequest
    {
        public required IFormFile File { get; set; }

        internal sealed class BulkUploadsValidator : Validator<BulkUploadsRequest>
        {
            public BulkUploadsValidator()
            {
                RuleFor(x => x.File)
                    .NotNull();

                RuleFor(x => Path.GetExtension(x.File.FileName))
                    .Equal("xlsm")
                    .WithMessage("File type is wrong. Please upload a .xslm file.");

                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo((long)1e+7)
                    .WithMessage("File size is larger than allowed");

                RuleFor(x => x.File.ContentType)
                    .NotNull()
                    .Must(x => x.Equals("application/octet-stream"))
                    .WithMessage("Invalid file type. Please upload a valid Excel file.");
            }
        }
    }

    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;
    }
}
