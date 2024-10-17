using InvoiceRequests.Add;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace AddInvoiceRequestAr
{
    internal sealed class AddArRequest : AddInvoiceRequestRequest
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

        internal sealed class Validator : Validator<AddArRequest>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class AddArResponse
    {
        public string Message { get; set; } = string.Empty;

        public InvoiceRequestAr? InvoiceRequest { get; set; }
    }
}
