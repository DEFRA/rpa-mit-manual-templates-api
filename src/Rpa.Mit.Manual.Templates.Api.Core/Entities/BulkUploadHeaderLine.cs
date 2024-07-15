using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadHeaderLine
    {
        public string InvoiceId { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public string TotalAmount { get; set; } = string.Empty;

        public string PreferredCurrency { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
