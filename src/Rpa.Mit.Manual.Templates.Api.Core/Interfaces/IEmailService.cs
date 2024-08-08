using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> EmailApprovers(IEnumerable<Approver> approvers, CancellationToken ct);
    }
}
