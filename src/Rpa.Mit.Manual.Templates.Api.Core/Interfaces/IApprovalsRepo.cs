using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApprovalsRepo
    {
        Task<bool> ApproveInvoice(Guid invoiceId, CancellationToken ct);
    }
}
