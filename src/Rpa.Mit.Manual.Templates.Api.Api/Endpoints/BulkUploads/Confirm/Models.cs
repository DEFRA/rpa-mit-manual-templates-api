using System.Diagnostics.CodeAnalysis;

namespace BulkUploadConfirm
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadConfirmRequest
    {
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// is the bulk upload confirmed or not
        /// </summary>
        public bool ConfirmUpload { get; set; }

        /// <summary>
        /// does user confirm the upload or not
        /// </summary>
        public bool Confirm { get; set; }

        internal sealed class Validator : Validator<BulkUploadConfirmRequest>
        {
            public Validator()
            {

            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadConfirmResponse
    {
        public string Message { get; set; } = string.Empty;

        public bool Result { get; set; }
    }
}
