using System.Data;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;


namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    internal sealed class InvoiceLineRepo : BaseData, IInvoiceLineRepo
    {
        public InvoiceLineRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<decimal> AddInvoiceLine(InvoiceLine invoiceLine, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {

                    try
                    {
                        var sql = "INSERT INTO invoicelines (id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybody, paymentrequestid )" +
                            " VALUES (@Id, @Value, @Description, @Fundcode, @mainaccount, @schemecode,  @marketingyear, @deliverybody, @paymentrequestid)";

                        await cn.ExecuteAsync(sql, invoiceLine);

                        var invoiceLineValues = await cn.QueryAsync<decimal>(
                                    "SELECT value FROM invoicelines WHERE paymentrequestid = @invoiceRequestId",
                                    new { InvoiceRequestId = invoiceLine.PaymentRequestId });

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
    }
}
