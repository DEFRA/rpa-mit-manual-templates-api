namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    public class BulkUploadBaseClass
    {
        /// <summary>
        /// calculated field = ClaimReferenceNumber-ClaimReference
        /// </summary>
        public string InvoiceId { get; set; } = string.Empty;

        public string ClaimReferenceNumber { get; set; } = string.Empty;

        public string ClaimReference { get; set; } = string.Empty;

        public string PreferredCurrency { get; set; } = string.Empty;
    }
}
