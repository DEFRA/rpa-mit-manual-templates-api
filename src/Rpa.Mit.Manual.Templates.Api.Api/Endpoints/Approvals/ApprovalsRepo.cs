using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals
{
    [ExcludeFromCodeCoverage]
    public class ApprovalsRepo : BaseData, IApprovalsRepo
    {
        public ApprovalsRepo() : base()
        { }

        public async Task<bool> ApproveInvoice(InvoiceApproval invoiceApproval, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoices SET approverid=@ApproverId,approveremail=@ApproverEmail,approved=TRUE,approvedby=@ApprovedBy,dateapproved=@DateApproved WHERE id = @id";

                var res = await cn.ExecuteAsync(sql, invoiceApproval);

                return res == 1;
            }
        }
    }
}
