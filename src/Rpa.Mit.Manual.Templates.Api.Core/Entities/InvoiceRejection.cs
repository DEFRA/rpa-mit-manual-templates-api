using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRejection
    {
        public Guid Id { get; set; }

        public DateTimeOffset DateApproved { get; set; }

        public string ApprovedBy { get; set; } = default!;

        public string Reason { get; set; } = default!;

        public Guid ApproverId { get; set; }

        public string ApproverEmail { get; set; } = default!;
    }
}
