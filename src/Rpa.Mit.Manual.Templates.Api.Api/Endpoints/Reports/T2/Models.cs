using System.Diagnostics.CodeAnalysis;

namespace T2Report
{
    [ExcludeFromCodeCoverage]
    internal sealed record Response
    {
        public IEnumerable<T2Report> Report { get; set; } = Enumerable.Empty<T2Report>();
    }

    [ExcludeFromCodeCoverage]
    internal sealed record T2Report
    {
        public string DeliveryBodyCode { get; set; } = string.Empty;

        public string Requester { get; set; } = string.Empty;

        public string Approver { get; set; } = string.Empty;

        public string InvoiceRequestId { get; set; } = string.Empty;

        public string LegacyId { get; set; } = string.Empty;

        public string Frn { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string SettlementDate { get; set; } = string.Empty;

        public string SettlementReference { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;






        public string Description { get; set; } = string.Empty;

        public string MarketingYear { get; set; } = string.Empty;

        public string FundCode { get; set; } = string.Empty;

        public string MainAccount { get; set; } = string.Empty;

        public string SchemeCode { get; set; } = string.Empty;

        public string DebtType { get; set; } = string.Empty;

    }
}
