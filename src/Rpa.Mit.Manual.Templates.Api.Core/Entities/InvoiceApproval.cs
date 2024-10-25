using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceApproval
    {
        public Guid Id { get; set; }

        public DateTimeOffset DateApproved { get; set; }

        public string ApproverEmail { get; set; } = default!;

        public string Status { get; set; } = default!;
    }
}
