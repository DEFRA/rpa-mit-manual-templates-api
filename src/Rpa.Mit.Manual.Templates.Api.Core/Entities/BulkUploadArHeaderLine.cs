using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class BulkUploadArHeaderLine : BulkUploadApHeaderLine
    {
        /// <summary>
        /// this is the AP invoice ID
        /// </summary>
        public string OriginalClaimReference { get; set; } = string.Empty;

        public string OriginalAPInvoiceSettlementDate { get; set; } = string.Empty;

        /// <summary>
        /// the earliest date when possible recovery was identified
        /// </summary>
        public string EarliestDatePossibleRecovery { get; set; } = string.Empty;

        /// <summary>
        /// Previous AR invoice ID. Only present if correcting a previous AR invoice
        /// </summary>
        public string CorrectionReference { get; set; } = string.Empty;


        public List<BulkUploadArDetailLine>? BulkUploadArDetailLines { get; set; }

    }
}
