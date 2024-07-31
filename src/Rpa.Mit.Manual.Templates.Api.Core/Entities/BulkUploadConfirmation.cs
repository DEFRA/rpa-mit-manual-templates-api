using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Enums;


namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadConfirmation
    {
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// is the bulk upload confirmed or not
        /// </summary>
        public bool Confirm { get; set; }

        public string Status { get; set; } = InvoiceStatuses.BulkUploadConfirmed;
    }
}
