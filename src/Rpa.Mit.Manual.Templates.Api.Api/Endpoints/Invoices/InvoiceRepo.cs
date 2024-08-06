using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;


using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRepo : BaseData, IInvoiceRepo
    {
        public InvoiceRepo() : base()
        { }

        public async Task<bool> AddInvoice(Invoice invoice, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"INSERT INTO Invoices (Id, SchemeType, Reference, Value, Status, CreatedBy, Created, PaymentType, AccountType, DeliveryBody, SecondaryQuestion, ApprovalGroup)
                                VALUES (@Id, @SchemeType, @Reference, @Value, @Status, @CreatedBy, @Created, @PaymentType, @AccountType, @DeliveryBody, @SecondaryQuestion, @ApprovalGroup)";

                var res = await cn.ExecuteAsync(sql, invoice);

                return res == 1;
            }
        }


        public async Task<Invoice> GetInvoiceForAzure(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,data,reference,value,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @Id";

                var parameters = new { Id = invoiceId };

                var invoice = await cn.QuerySingleAsync<Invoice>(sql, parameters);

                var prSql = "SELECT invoiceid, invoicerequestid, frn, sbi, vendor, agreementnumber, currency, description, value, marketingyear, duedate FROM invoicerequests WHERE invoiceid = @Id";
                var prParameters = new { invoice.Id };
                invoice.InvoiceRequests = await cn.QueryAsync<InvoiceRequest>(prSql, prParameters);

                foreach (InvoiceRequest pr in invoice.InvoiceRequests)
                {
                    // get the invoice lines
                    var invSql = "SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid FROM invoicelines WHERE invoicerequestid = @invoicerequestid";
                    var invParameters = new { invoicerequestid = pr.InvoiceRequestId };
                    pr.InvoiceLines = await cn.QueryAsync<InvoiceLine>(invSql, invParameters);
                }

                return invoice;
            }
        }

        public async Task<bool> DeleteInvoice(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        // get all invoicerequest ids
                        var invoiceRequestIds = await cn.QueryAsync<string>(
                            "SELECT invoicerequestid FROM invoicerequests WHERE invoiceid = @InvoiceId",
                            new { InvoiceId = invoiceId },
                            transaction: transaction);

                        // for each invoiceRequestId, delete all invoice lines
                        foreach (string invoiceRequestId in invoiceRequestIds)
                        {
                            await cn.ExecuteAsync(
                                    "DELETE FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                                    new { invoiceRequestId},
                                    transaction: transaction);
                        }

                        // for each invoiceRequestId, delete the invoice request
                        foreach (string invoiceRequestId in invoiceRequestIds)
                        {
                            await cn.ExecuteAsync(
                                    "DELETE FROM invoicerequests WHERE invoicerequestid = @invoiceRequestId",
                                    new { invoiceRequestId },
                                    transaction: transaction);
                        }

                        // finally, delete the invoice header
                        await cn.ExecuteAsync(
                                "DELETE FROM invoices WHERE id = @invoiceId",
                                new { invoiceId },
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

        public async Task<IEnumerable<Invoice>> GetAllInvoices(CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,data,reference,value,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices";


                return await cn.QueryAsync<Invoice>(sql);
            }
        }

        public async Task<Invoice> GetInvoiceByInvoiceId(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,data,reference,value,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @invoiceid";

                var parameters = new { invoiceId };

                return await cn.QuerySingleAsync<Invoice>(sql, parameters);
            }
        }
    }
}
