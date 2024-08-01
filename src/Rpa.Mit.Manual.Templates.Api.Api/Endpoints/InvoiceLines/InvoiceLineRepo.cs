using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;


namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    [ExcludeFromCodeCoverage]
    internal sealed class InvoiceLineRepo : BaseData, IInvoiceLineRepo
    {
        public InvoiceLineRepo() : base()
        { }

        public async Task<decimal> AddInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        var sql = "INSERT INTO invoicelines (id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid )" +
                            " VALUES (@Id, @Value, @Description, @Fundcode, @mainaccount, @schemecode,  @marketingyear, @deliverybody, @invoicerequestid)";

                        await cn.ExecuteAsync(sql, invoiceLine);

                        var invoiceLineValues = await cn.QueryAsync<decimal>(
                                    "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                                    new { invoiceLine.InvoiceRequestId });

                        await transaction.CommitAsync(ct);

                        return invoiceLineValues.Sum();
                    }
                    catch
                    {
                        await transaction.RollbackAsync(ct);
                        throw; 
                    }
                }
            }
        }

        public async Task<decimal> DeleteInvoiceLine(Guid invoiceLineId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        // get parent invoicerequest id
                        var invoiceRequestId = await cn.QuerySingleAsync<string>(
                            "SELECT invoicerequestid FROM invoicelines WHERE id = @invoiceLineId",
                            new { invoiceLineId },
                            transaction: transaction);

                        // delete our invoice line
                        await cn.ExecuteAsync(
                                "DELETE FROM invoicelines WHERE id = @invoiceLineId",
                                new { invoiceLineId },
                                transaction: transaction);

                        // get the new total value and return
                        var invoiceLineValues = await cn.QueryAsync<decimal>(
                                    "SELECT value FROM invoicelines WHERE invoicerequestid = @invoicerequestid",
                                    new { invoiceRequestId });

                        await transaction.CommitAsync(ct);

                        return invoiceLineValues.Sum();
                    }
                    catch
                    {
                        await transaction.RollbackAsync(ct);
                        throw;
                    }
                }
            }
        }


        public async Task<decimal> UpdateInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        var sql = "UPDATE invoicelines SET value = @Value, description = @Description, fundcode = @Fundcode, mainaccount = @mainaccount, schemecode = @schemecode, marketingyear = @marketingyear, deliverybody = @deliverybody WHERE id = @id";

                        await cn.ExecuteAsync(sql, invoiceLine);

                        var invoiceLineValues = await cn.QueryAsync<decimal>(
                                    "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                                    new { invoiceLine.InvoiceRequestId });

                        await transaction.CommitAsync(ct);

                        return invoiceLineValues.Sum();
                    }
                    catch
                    {
                        await transaction.RollbackAsync(ct);
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<InvoiceLine>> GetInvoiceLinesByInvoiceRequestId(string invoiceRequestId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                return  await cn.QueryAsync<InvoiceLine>(
                            "SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid FROM invoicelines WHERE invoiceRequestId = @invoiceRequestId",
                            new { invoiceRequestId });
            }
        }

        public async Task<InvoiceLine> GetInvoiceLineByInvoiceLineId(Guid invoiceLineId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                return await cn.QuerySingleAsync<InvoiceLine>(
                            "SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid FROM invoicelines WHERE id = @invoiceLineId",
                            new { invoiceLineId });
            }
        }
    }
}
