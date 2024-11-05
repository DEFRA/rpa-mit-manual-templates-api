using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApproversAdminRepo
    {
        Task<int> Createlookup_approver(string email, string deliverybody, string schemecode, int? threshold);
        Task<int> Deletelookup_approver(string email, string deliverybody, string schemecode, int? threshold);
        Task<IEnumerable<AdminApprover>> FindAlllookup_approvers();
        Task<IEnumerable<AdminApprover>> Findlookup_approversByAll(string email, string deliverybody, string schemecode, int? threshold);
        Task<IEnumerable<AdminApprover>> Findlookup_approversByAny(string email, string deliverybody, string schemecode, int? threshold);
        Task<AdminApprover> Getlookup_approver(string email, string deliverybody);
        Task<int> Updatelookup_approver(string email, string deliverybody, string schemecode, int? threshold);
    }
}