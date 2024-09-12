using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceRepo : BaseData, IInvoiceRepo
    {
        public InvoiceRepo(IOptions<PostGres> options) : base(options)
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

        public async Task<string> GetInvoiceCreatorEmailAddress(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT createdby FROM invoices WHERE Id = @invoiceid";

                var parameters = new { invoiceId };

                return await cn.QuerySingleAsync<string>(sql, parameters);
            }
        }
    }
}
