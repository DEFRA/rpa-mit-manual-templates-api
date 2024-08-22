using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRejection
    {
        public Guid Id { get; set; }

        public DateTimeOffset DateApproved { get; set; }

        public string Reason { get; set; } = default!;

        public string ApproverEmail { get; set; } = default!;
    }
}
