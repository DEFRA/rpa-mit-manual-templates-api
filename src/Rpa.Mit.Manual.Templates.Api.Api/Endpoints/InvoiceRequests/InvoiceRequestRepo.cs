using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestRepo : BaseData, IInvoiceRequestRepo
    {
        public InvoiceRequestRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<bool> AddInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "INSERT INTO invoicerequests (invoicerequestid, invoiceid, ledger, frn, sbi, vendor, marketingyear, agreementnumber, currency, description, duedate, claimreferencenumber, claimreference )" +
                     " VALUES (@InvoiceRequestId, @InvoiceId, @Ledger, @Frn, @Sbi, @Vendor, @MarketingYear,  @AgreementNumber, @Currency, @Description, @DueDate, @claimreferencenumber, @claimreference)";

                var res = await cn.ExecuteAsync(sql, invoiceRequest);

                return res == 1;
            }
        }

        public async Task<decimal> GetInvoiceRequestValue(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var invoiceLineValues = await cn.QueryAsync<decimal>(
                            "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                            new { InvoiceRequestId = invoiceRequestId });

                return invoiceLineValues.Sum();
            }
        }

        public async Task<bool> UpdateInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoicerequests SET frn=@Frn, sbi=@Sbi, vendor=@Vendor, agreementnumber=@AgreementNumber, currency=@Currency, description=@Description, marketingyear=@MarketingYear, duedate=@DueDate, claimreferencenumber=@ClaimReferenceNumber, claimreference=@ClaimReference WHERE invoicerequestid = @InvoiceRequestId";

                var res = await cn.ExecuteAsync(sql, invoiceRequest);

                return res == 1;
            }
        }

        public async Task<bool> UpdateInvoiceRequestStatus(string invoiceRequestId, string status, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "UPDATE invoicerequests SET status=@status WHERE invoicerequestid=@InvoiceRequestId";

                var res = await cn.ExecuteAsync(sql, new { invoiceRequestId, status });

                return res == 1;
            }
        }

        public async Task<bool> DeleteInvoiceRequest(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        // first delete all children
                        await cn.ExecuteAsync(
                                "DELETE FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                                new { invoiceRequestId },
                                transaction: transaction);

                        // now delete the parent invoice request
                        await cn.ExecuteAsync(
                                "DELETE FROM invoicerequests WHERE invoicerequestid = @invoiceRequestId",
                                new { invoiceRequestId },
                                transaction: transaction);

                        await transaction.CommitAsync(ct);

                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync(ct);
                        throw;
                    }
                }
            }
        }
         
        public async Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsByInvoiceId(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"
                            SELECT frn,sbi,vendor,agreementnumber,currency,ir.description,ir.invoicerequestid,il.marketingyear,duedate,claimreferencenumber,claimreference,invoiceid,
                            SUM(il.value) AS value
                            FROM invoicerequests ir LEFT JOIN invoicelines il 
                            ON ir.invoicerequestid = il.invoicerequestid
                            WHERE ir.invoiceid = @invoiceId
                            group by frn, sbi, vendor,agreementnumber,currency,ir.description,ir.invoicerequestid,il.marketingyear,duedate,claimreferencenumber,claimreference,invoiceid
                          ";

                return await cn.QueryAsync<InvoiceRequest>(
                            sql,
                            new { invoiceId });
            }
        }

        public async Task<InvoiceRequest> GetInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                InvoiceRequest invoiceRequest = new InvoiceRequest();

                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                invoiceRequest = await cn.QuerySingleAsync<InvoiceRequest>(
                            "SELECT frn, sbi, vendor, agreementnumber, currency, description, invoicerequestid, marketingyear, duedate, claimreferencenumber, claimreference, invoiceid FROM invoicerequests WHERE invoicerequestid = @invoiceRequestId",
                            new { invoiceRequestId });

                var invoiceLineValues = await cn.QueryAsync<decimal>(
                            "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                            new { InvoiceRequestId = invoiceRequestId });

                invoiceRequest.Value = invoiceLineValues.Sum();

                return invoiceRequest;
            }
        }

        public async Task<bool> UpdateInvoiceRequestWithPaymentHubResponse(PaymentHubResponseForDatabase paymentHubResponseForDatabase)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                var sql = "UPDATE invoicerequests SET paymenthubdateprocessed=@paymenthubdateprocessed,paymenthuberror=@error,paymenthubaccepted=@accepted WHERE invoicerequestid=@invoicerequestid";

                var res = await cn.ExecuteAsync(sql, paymentHubResponseForDatabase);

                return res == 1;
            }
        }

        public async Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsThatHaveErroredInPaymentHub(CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                return await cn.QueryAsync<InvoiceRequest>("SELECT frn, sbi, vendor, agreementnumber, currency, description, value, invoicerequestid, marketingyear, duedate, claimreferencenumber, claimreference, invoiceid FROM invoicerequests WHERE paymenthuberroremailsent is null");
            }
        }

        public async Task<bool> AddInvoiceRequestAr(InvoiceRequestAr invoiceRequest, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "INSERT INTO invoicerequests (invoicerequestid, invoiceid, ledger, frn, sbi, vendor, marketingyear, agreementnumber, currency, description, duedate, claimreferencenumber, claimreference,originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference )" +
                     " VALUES (@InvoiceRequestId,@InvoiceId,@Ledger,@Frn,@Sbi,@Vendor,@MarketingYear,@AgreementNumber,@Currency,@Description,@DueDate,@claimreferencenumber,@claimreference,@OriginalClaimReference,@OriginalAPInvoiceSettlementDate,@EarliestDatePossibleRecovery,@CorrectionReference)";

                var res = await cn.ExecuteAsync(sql, invoiceRequest);

                return res == 1;
            }
        }

        public async Task<IEnumerable<InvoiceRequestAr>> GetArInvoiceRequestsByInvoiceId(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"
                            SELECT frn,sbi,vendor,agreementnumber,currency,ir.description,ir.invoicerequestid,il.marketingyear,duedate,claimreferencenumber,claimreference,invoiceid,originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference,
                                    SUM(il.value) AS value
                                FROM invoicerequests ir LEFT JOIN invoicelines il 
                                ON ir.invoicerequestid = il.invoicerequestid
                                WHERE ir.invoiceid = @invoiceId
                                GROUP BY frn, sbi, vendor,agreementnumber,currency,ir.description,ir.invoicerequestid,il.marketingyear,duedate,claimreferencenumber,claimreference,invoiceid,
                                    originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference
                          ";

                return await cn.QueryAsync<InvoiceRequestAr>(
                            sql,
                            new { invoiceId });
            }
        }

        public async Task<InvoiceRequestAr> GetArInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                InvoiceRequestAr invoiceRequest = new InvoiceRequestAr();

                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                invoiceRequest = await cn.QuerySingleAsync<InvoiceRequestAr>(
                            "SELECT frn, sbi, vendor, agreementnumber, currency, description, invoicerequestid, marketingyear, duedate, claimreferencenumber, claimreference, invoiceid, originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference FROM invoicerequests WHERE invoicerequestid = @invoiceRequestId",
                            new { invoiceRequestId });

                var invoiceLineValues = await cn.QueryAsync<decimal>(
                            "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                            new { InvoiceRequestId = invoiceRequestId });

                invoiceRequest.Value = invoiceLineValues.Sum();

                return invoiceRequest;
            }
        }

        public async Task<bool> UpdateInvoiceRequestApprovalStatus(List<string> invoiceRequestIds, Guid invoiceId, string approver, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

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

        public async Task<IEnumerable<InvoiceRequestForAzure>> GetInvoiceRequestsForAzure(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var invApSql = "SELECT schemetype,reference,deliverybody FROM invoices WHERE id = @invoiceId";
                var invApParameters = new { invoiceId };
                var invoice = await cn.QuerySingleAsync<Invoice>(invApSql, invApParameters);

                var prApSql = "SELECT invoicerequestid,ledger,frn,currency,marketingyear,claimreference AS invoiceNumber FROM invoicerequests WHERE invoiceid = @invoiceId";
                var prApParameters = new { invoiceId };
                var invoiceRequestsAp = await cn.QueryAsync<InvoiceRequestForAzure>(prApSql, prApParameters);

                foreach (InvoiceRequestForAzure invoiceRequestAp in invoiceRequestsAp)
                {
                    invoiceRequestAp.invoiceNumber = invoiceId.ToString();
                    invoiceRequestAp.deliveryBody = invoice.DeliveryBody;
                    invoiceRequestAp.agreementNumber = "TEST-AP;
                    invoiceRequestAp.paymentRequestNumber = 10;

                    // get the invoice lines
                    var invLineSql = "SELECT value, description, fundcode, mainaccount AS accountCode, schemecode, marketingyear, deliverybodycode FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invLineParameters = new { invoicerequestid = invoiceRequestAp.InvoiceRequestId };
                    invoiceRequestAp.invoiceLines = await cn.QueryAsync<InvoiceLineForAzure>(invLineSql, invLineParameters);

                    invoiceRequestAp.value = invoiceRequestAp.invoiceLines.Sum(x => x.value);
                }

                return invoiceRequestsAp;
            }
        }

        public async Task<IEnumerable<InvoiceRequestArForAzure>> GetInvoiceRequestsArForAzure(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var invSql = "SELECT schemetype,reference,deliverybody FROM invoices WHERE id = @invoiceId";
                var invParameters = new { invoiceId };
                var invoice = await cn.QuerySingleAsync<Invoice>(invSql, invParameters);

                var prSql = "SELECT invoiceid,invoicerequestid,ledger,frn,currency,marketingyear,claimreference AS invoiceNumber,sbi,vendor,agreementnumber,description,value,duedate,claimreferencenumber,originalclaimreference,originalapinvoicesettlementdate,earliestdatepossiblerecovery,correctionreference FROM invoicerequests WHERE invoiceid = @invoiceId";
                var prParameters = new { invoiceId };
                var invoiceRequestsAr = await cn.QueryAsync<InvoiceRequestArForAzure>(prSql, prParameters);

                foreach (InvoiceRequestArForAzure invoiceRequestAr in invoiceRequestsAr)
                {
                    invoiceRequestAr.invoiceNumber = invoiceId.ToString();
                    invoiceRequestAr.deliveryBody = invoice.DeliveryBody;
                    invoiceRequestAr.agreementNumber = "TEST-AR";
                    invoiceRequestAr.paymentRequestNumber = 1;

                    // get the invoice lines
                    var invLineSql = "SELECT value, description, debttype, fundcode, mainaccount AS accountCode, schemecode, marketingyear, deliverybodycode FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invLineParms = new { invoicerequestid = invoiceRequestAr.InvoiceRequestId };
                    invoiceRequestAr.invoiceLines = await cn.QueryAsync<InvoiceLineForAzureAr>(invLineSql, invLineParms);

                    invoiceRequestAr.value = invoiceRequestAr.invoiceLines.Sum(x => x.value);
                }

                return invoiceRequestsAr;
            }
        }
    }
}
