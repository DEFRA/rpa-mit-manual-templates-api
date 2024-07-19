namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// used to get the text values for the detail line description during a bulk import
    /// </summary>
    public class BulkUploadDetailLineDescriptionQuery
    {
        public string MainAccount { get; set; } = string.Empty;

        public string Scheme { get; set; } = string.Empty;

        public string DeliveryBodyCode { get; set; } = string.Empty;
    }
}
