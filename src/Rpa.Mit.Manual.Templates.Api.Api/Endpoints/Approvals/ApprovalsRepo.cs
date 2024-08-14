using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
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

        public async Task<IEnumerable<InvoiceRequestForAzure>> GetInvoiceRequestsForAzure(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var prSql = "SELECT invoicerequestid, frn, sbi, vendor, agreementnumber, currency, value, marketingyear FROM invoicerequests WHERE invoiceid = @invoiceId";
                var prParameters = new { invoiceId };
                var invoiceRequests = await cn.QueryAsync<InvoiceRequestForAzure>(prSql, prParameters);

                foreach (InvoiceRequestForAzure pr in invoiceRequests)
                {
                    // get the invoice lines
                    var invSql = "SELECT value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invParameters = new { invoicerequestid = pr.InvoiceRequestId };
                    pr.InvoiceLines = await cn.QueryAsync<InvoiceLineForAzure>(invSql, invParameters);

                    pr.Value = pr.InvoiceLines.Sum(x => x.Value);
                }

                return invoiceRequests;
            }
        }

        public async Task<bool> RejectInvoice(InvoiceRejection invoiceRejection, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoices SET approverid=@ApproverId,approveremail=@ApproverEmail,approved=FALSE,approvedby=@ApprovedBy,dateapproved=@DateApproved,approvalrejectionreason=@Reason WHERE id = @id";

                var res = await cn.ExecuteAsync(sql, invoiceRejection);

                return res == 1;
            }
        }
    }
}
