using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace GetMyApprovals
{
    internal sealed class GetMyApprovalsResponse
    {
        public IEnumerable<Invoice> Invoices { get; set; } = Enumerable.Empty<Invoice>();

        public string Message { get; set; } = string.Empty;
    }
}
