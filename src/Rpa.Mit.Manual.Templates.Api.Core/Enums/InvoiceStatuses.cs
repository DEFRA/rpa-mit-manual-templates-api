using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Enums
{
    [ExcludeFromCodeCoverage]
    public static class InvoiceStatuses
    {
        public const string New = "new";
        public const string BulkUploadConfirmed = "BULKUPLOAD_CONFIRMED";
        public const string AwaitingApproval = "AWAITING_APPROVAL";
        public const string Approved = "APPROVED";
        public const string Rejected = "REJECTED";

        public static string DisplayNameFor(string status)
        {
            switch (status)
            {
                case New:
                    return "New";
                case BulkUploadConfirmed:
                    return "BulkUpload Confirmed";
                case AwaitingApproval:
                    return "Awaiting Approval";
                case Approved:
                    return "Approved";
                case Rejected:
                    return "Rejected";
                default:
                    return status;
            }
        }
    }
}
