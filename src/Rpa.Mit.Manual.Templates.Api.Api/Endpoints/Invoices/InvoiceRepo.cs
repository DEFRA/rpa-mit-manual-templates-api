using Npgsql;
using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Microsoft.Extensions.Options;
using Dapper;
using System.Diagnostics.CodeAnalysis;


namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRepo : BaseData, IInvoiceRepo
    {
        public InvoiceRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> AddInvoice(Invoice invoice, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = @"INSERT INTO Invoices (Id, SchemeType, Reference, Value, Status, CreatedBy, Created, PaymentType, AccountType, DeliveryBody, SecondaryQuestion)
                                VALUES (@Id, @SchemeType, @Reference, @Value, @Status, @CreatedBy, @Created, @PaymentType, @AccountType, @DeliveryBody, @SecondaryQuestion)";

                var res = await cn.ExecuteAsync(sql, invoice);

                return res == 1;
            }
        }


        public async Task<Invoice> GetInvoiceForAzure(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,data,reference,value,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @Id";

                var parameters = new { Id = invoiceId };

                var invoice = await cn.QuerySingleAsync<Invoice>(sql, parameters);

                var prSql = "SELECT invoiceid, paymentrequestid, frn, sbi, vendor, agreementnumber, currency, description, value, marketingyear, duedate FROM paymentrequests WHERE invoiceid = @Id";
                var prParameters = new { Id = invoice.Id };
                invoice.PaymentRequests = await cn.QueryAsync<InvoiceRequest>(prSql, prParameters);

                foreach (InvoiceRequest pr in invoice.PaymentRequests)
                {
                    // get the invoice lines
                    var invSql = "SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybody, paymentrequestid FROM invoicelines WHERE paymentrequestid = @paymentrequestid";
                    var invParameters = new { paymentrequestid = pr.InvoiceRequestId };
                    pr.InvoiceLines = await cn.QueryAsync<InvoiceLine>(invSql, invParameters);
                }

                return invoice;
            }
        }

        public async Task<bool> DeleteInvoice(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync())
                {
                    try
                    {
                        // get all payment request ids
                        var prIds = await cn.QueryAsync<string>(
                            "SELECT paymentrequestid FROM paymentrequests WHERE invoiceid = @InvoiceId",
                            new { InvoiceId = invoiceId },
                            transaction: transaction);

                        // for each pr id, delete all invoice lines
                        foreach (string prId in prIds)
                        {
                            await cn.ExecuteAsync(
                                    "DELETE FROM invoicelines WHERE paymentrequestid = @prId",
                                    new { prId = prId},
                                    transaction: transaction);
                        }

                        // for each pr id, delete the payment request
                        foreach (string prId in prIds)
                        {
                            await cn.ExecuteAsync(
                                    "DELETE FROM paymentrequests WHERE paymentrequestid = @prId",
                                    new { prId = prId },
                                    transaction: transaction);
                        }

                        // finally, delete the invoice header
                        await cn.ExecuteAsync(
                                "DELETE FROM invoices WHERE id = @InvoiceId",
                                new { InvoiceId = invoiceId },
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
    }
}
