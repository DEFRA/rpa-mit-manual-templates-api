using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.InvoiceRequests
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRequestRepo : BaseData, IInvoiceRequestRepo
    {
        public InvoiceRequestRepo() : base()
        { }

        public async Task<bool> AddInvoiceRequest(InvoiceRequest invoiceRequest, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "INSERT INTO invoicerequests (invoicerequestid, invoiceid, frn, sbi, vendor, marketingyear, agreementnumber, currency, description, duedate, value, claimreferencenumber, claimreference )" +
                     " VALUES (@InvoiceRequestId, @InvoiceId, @Frn, @Sbi, @Vendor, @MarketingYear,  @AgreementNumber, @Currency, @Description, @DueDate, @Value, @claimreferencenumber, @claimreference)";

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

                return await cn.QueryAsync<InvoiceRequest>(
                            "SELECT frn, sbi, vendor, agreementnumber, currency, description, value, invoicerequestid, marketingyear, duedate, claimreferencenumber, claimreference, invoiceid FROM invoicerequests WHERE invoiceid = @invoiceId",
                            new { invoiceId });
            }
        }

        public async Task<InvoiceRequest> GetInvoiceRequestByInvoiceRequestId(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                return await cn.QuerySingleAsync<InvoiceRequest>(
                            "SELECT frn, sbi, vendor, agreementnumber, currency, description, value, invoicerequestid, marketingyear, duedate, claimreferencenumber, claimreference, invoiceid FROM invoicerequests WHERE invoicerequestid = @invoiceRequestId",
                            new { invoiceRequestId });
            }
        }
    }
}
