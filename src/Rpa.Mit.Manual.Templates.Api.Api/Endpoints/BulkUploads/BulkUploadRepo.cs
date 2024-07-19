using System.Data;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    public class BulkUploadRepo : BaseData, IBulkUploadRepo
    {
        public BulkUploadRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> AddApBulkUpload(BulkUploadApDataset bulkUploadApDataset, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        var sql = "INSERT INTO invoicelines (id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybody, invoicerequestid )" +
                            " VALUES (@Id, @Value, @Description, @Fundcode, @mainaccount, @schemecode,  @marketingyear, @deliverybody, @invoicerequestid)";

                        await cn.ExecuteAsync(sql, bulkUploadApDataset);

                        //var invoiceLineValues = await cn.QueryAsync<decimal>(
                        //            "SELECT value FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                        //            new { invoiceLine.InvoiceRequestId });

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

        public async Task<string> GetDetailLineDescripions(string query, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        var mainAccount = await cn.QuerySingleAsync<string>(
                                    "SELECT description FROM lookup_ap_chartofaccounts WHERE code = @query",
                                    new { query });

                        await transaction.CommitAsync(ct);

                        return mainAccount;
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
