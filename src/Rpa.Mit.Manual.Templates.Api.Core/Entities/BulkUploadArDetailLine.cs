using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadArDetailLine
    {
        public Guid Id { get; set; }

        public string InvoiceRequestId { get; set; } = string.Empty;

        public decimal Value { get; set; } = 0.0M;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string DeliveryBodyCode { get; set; } = string.Empty;

        /// <summary>
        /// calculated field = Main Account AP / 
        ///  strDES = strACC & " / " & strSCH & " / " & strDB 
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
