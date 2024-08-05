using System.Data;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> EmailApprovers(IEnumerable<string> approvers, CancellationToken ct);
    }
}
