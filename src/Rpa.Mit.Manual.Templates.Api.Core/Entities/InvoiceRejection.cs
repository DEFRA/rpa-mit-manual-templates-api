using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class InvoiceRejection
    {
        public Guid Id { get; set; }

        public DateTimeOffset DateRejected { get; set; }

        public string RejectedBy { get; set; } = default!;

        public string Reason { get; set; } = default!;
    }
}
