﻿using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    /// <summary>
    /// An invoice request for an AR type
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRequestAr : InvoiceRequestBase
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

        public IEnumerable<InvoiceLineAr>? InvoiceLinesAr { get; set; } = Enumerable.Empty<InvoiceLineAr>();
    }
}