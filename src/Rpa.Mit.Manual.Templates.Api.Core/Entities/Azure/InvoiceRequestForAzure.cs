using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestForAzure
    {
        [JsonIgnore]
        public string InvoiceRequestId { get; set; } = string.Empty;

        public string SourceSystem { get; set; } = "Manual";

        public string FRN { get; set; } = string.Empty;

        public string SBI { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public int InvoiceRequestNumber { get; set; }

        public string AgreementNumber { get; set; } = string.Empty;

        public string Currency { get; set; } = "GBP";

        public string DueDate { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string AccountType { get; set; } = string.Empty;

        public string Vendor { get; set; } = string.Empty;

        public IEnumerable<InvoiceLineForAzure> InvoiceLines { get; set; } = Enumerable.Empty<InvoiceLineForAzure>();
    }
}
