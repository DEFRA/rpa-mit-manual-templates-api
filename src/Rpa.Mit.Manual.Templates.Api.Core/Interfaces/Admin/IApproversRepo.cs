using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApproversAdminRepo
    {
        Task<bool> Create(AdminApprover adminApprover, CancellationToken ct);
        Task<bool> Delete(string email, string deliverybody, CancellationToken ct);   
        Task<IEnumerable<AdminApprover>> GetAll(CancellationToken ct);
        Task<IEnumerable<AdminApprover>> GetByAll(string? email, string? deliverybody, string? schemetype, int? threshold, CancellationToken ct);
        Task<IEnumerable<AdminApprover>> GetByAny(string email, string deliverybody, string schemetype, int? threshold, CancellationToken ct);
        Task<AdminApprover> Get(string email, string deliverybody, CancellationToken ct);
        Task<bool> Update(AdminApprover adminApprover, CancellationToken ct);
    }
}