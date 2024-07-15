using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadApDetailLine : BulkUploadBaseClass
    {
        public string Amount { get; set; } = string.Empty;

        public string Fund { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string Scheme { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBodyCode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
