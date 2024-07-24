using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Options;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals
{
    [ExcludeFromCodeCoverage]
    public class ApprovalsRepo : BaseData, IApprovalsRepo
    {
        public ApprovalsRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> ApproveInvoice(Guid invoiceId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
