using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    [ExcludeFromCodeCoverage]
    public interface IApprovalsRepo
    {
        Task<bool> ApproveInvoice(Guid invoiceId, CancellationToken ct);
    }
}
