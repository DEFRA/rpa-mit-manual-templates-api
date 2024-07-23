namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    public class BulkUploadBaseClass
    {
        /// <summary>
        /// calculated field = ClaimReferenceNumber-ClaimReference
        /// </summary>
        public Guid InvoiceId { get; set; }


        public string PreferredCurrency { get; set; } = string.Empty;
    }
}
