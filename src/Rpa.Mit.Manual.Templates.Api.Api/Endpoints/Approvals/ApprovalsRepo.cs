using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Approvals
{
    [ExcludeFromCodeCoverage]
    public class ApprovalsRepo : BaseData, IApprovalsRepo
    {
        public ApprovalsRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<bool> ApproveInvoice(InvoiceApproval invoiceApproval, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoices SET approveremail=@ApproverEmail,status = @Status, approved=TRUE,dateapproved=@DateApproved WHERE id = @id";

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

                // filter against logged-in approver
                var sql = "SELECT id,schemetype,reference,status,createdby,created,paymenttype,accounttype,deliverybody FROM invoices WHERE approveremail = @approverEmail";

                var invoices = await cn.QueryAsync<Invoice>(sql, approverEmail);

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

        public async Task<Invoice> GetInvoiceForApproval(Guid invoiceId, string approverEmail, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,reference,createdby, updatedby, created,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @Id";

                var parameters = new { Id = invoiceId };

                var invoice = await cn.QuerySingleAsync<Invoice>(sql, parameters);

                var prSql = "SELECT invoiceid, invoicerequestid, frn, sbi, vendor, agreementnumber, currency, description, value, marketingyear, duedate FROM invoicerequests WHERE invoiceid = @Id";
                var prParameters = new { invoice.Id };
                invoice.InvoiceRequests = await cn.QueryAsync<InvoiceRequest>(prSql, prParameters);

                foreach (InvoiceRequest pr in invoice.InvoiceRequests)
                {
                    // get the invoice detail lines
                    var invSql = "SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invParameters = new { invoicerequestid = pr.InvoiceRequestId };
                    pr.InvoiceLines = await cn.QueryAsync<InvoiceLine>(invSql, invParameters);

                    pr.Value = pr.InvoiceLines.Select(c => c.Value).Sum();

                    invoice.Value += pr.Value;
                }

                return invoice;
            }
        }

        public async Task<bool> RejectInvoice(InvoiceRejection invoiceRejection, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoices SET status=@Status, approverid=@ApproverId,approveremail=@ApproverEmail,approved=FALSE,approvedby=@ApprovedBy,dateapproved=@DateApproved,approvalrejectionreason=@Reason WHERE id = @id";

                var res = await cn.ExecuteAsync(sql, invoiceRejection);

                return res == 1;
            }
        }

        public async Task<InvoiceAr> GetInvoiceArForApproval(Guid invoiceId, string approverEmail, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,reference,createdby, updatedby, created,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @Id";

                var parameters = new { Id = invoiceId };

                var invoice = await cn.QuerySingleAsync<InvoiceAr>(sql, parameters);

                var prSql = "SELECT invoiceid,invoicerequestid,frn,sbi,vendor,agreementnumber,currency,description,value,marketingyear,duedate,claimreferencenumber,claimreference,invoiceid,paymenthuberror,paymenthubaccepted,paymenthubdateprocessed,paymenthuberroremailsent,leger,originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference FROM invoicerequests WHERE invoiceid = @Id";
                var prParameters = new { invoice.Id };
                invoice.InvoiceRequests = await cn.QueryAsync<InvoiceRequestAr>(prSql, prParameters);

                foreach (InvoiceRequestAr pr in invoice.InvoiceRequests)
                {
                    // get the invoice detail lines
                    var invSql = "SELECT id,value,description,fundcode,mainaccount,schemecode,marketingyear,deliverybodycode,invoicerequestid,debttype FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invParameters = new { invoicerequestid = pr.InvoiceRequestId };
                    pr.InvoiceLinesAr = await cn.QueryAsync<InvoiceLineAr>(invSql, invParameters);

                    pr.Value = pr.InvoiceLinesAr.Select(c => c.Value).Sum();

                    invoice.Value += pr.Value;
                }

                return invoice;
            }
        }

        public async Task<bool> UpdateInvoiceRequestApprovalStatus(List<string> invoiceRequestIds, Guid invoiceId, string approver, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var dateApproved = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("UPDATE invoices SET approveremail='{0}',dateapproved='{1}' WHERE id='{2}';", approver, dateApproved, invoiceId);

                foreach (string invoiceRequestId in invoiceRequestIds)
                {
                    sb.AppendFormat("UPDATE invoicerequests SET approver='{0}',dateapproved='{1}' WHERE invoicerequestid='{2}';", approver, dateApproved, invoiceRequestId);
                }

                await cn.ExecuteAsync(sb.ToString());

                return true;
            }
        }
    }
}
