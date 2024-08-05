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

        public async Task<IEnumerable<Invoice>> GetMyApprovals(string approverEmail, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                //TODO: needs to be refined to filter against logged-in approver
                var sql = "SELECT id,schemetype,reference,value,status,createdby,created,paymenttype,accounttype,deliverybody FROM invoices";

                var invoices = await cn.QueryAsync<Invoice>(sql);

                // get the values of child invoice requests and sum them
                foreach (var invoice in invoices)
                {
                    var invoiceLimeValues = await cn.QueryAsync<decimal>(
                            "select value from public.invoicelines where invoicerequestid in (Select invoicerequestid from invoicerequests where invoiceid=@Id)",
                            new { invoice.Id});

                    invoice.Value = invoiceLimeValues.Sum();
                }

                return invoices;
            }
        }
    }
}
