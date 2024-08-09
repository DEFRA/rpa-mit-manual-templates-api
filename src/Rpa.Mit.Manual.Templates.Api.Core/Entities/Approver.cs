using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public struct Approver
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public struct SelectedApprover
    {
        public Guid InvoiceId { get; set; }

        public Guid ApproverId { get; set; }
    }
}
