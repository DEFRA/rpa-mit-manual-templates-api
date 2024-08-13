using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    [ExcludeFromCodeCoverage]
    public class BulkUploadRepo : BaseData, IBulkUploadRepo
    {
        public BulkUploadRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<bool> AddApBulkUpload(BulkUploadApDataset bulkUploadApDataset, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {

                        var sql = @"INSERT INTO Invoices (Id, SchemeType, Reference, Value, Status, CreatedBy, Created, PaymentType, AccountType, DeliveryBody, SecondaryQuestion, ApprovalGroup)
                                VALUES (@Id, @SchemeType, @Reference, @Value, @Status, @CreatedBy, @Created, @PaymentType, @AccountType, @DeliveryBody, @SecondaryQuestion, @ApprovalGroup)";

                        await cn.ExecuteAsync(sql, bulkUploadApDataset.BulkUploadInvoice);

                        var sql1 = "INSERT INTO invoicerequests (invoicerequestid, invoiceid, frn, sbi, vendor, marketingyear, agreementnumber, currency, description, duedate, claimreferencenumber, claimreference )" +
                             " VALUES (@InvoiceRequestId, @InvoiceId, @Frn, @Sbi, @Vendor, @MarketingYear,  @AgreementNumber, @PaymentType, @Description, @DueDate, @claimreferencenumber, @claimreference)";

                        await cn.ExecuteAsync(sql1, bulkUploadApDataset.BulkUploadInvoice!.BulkUploadApHeaderLines);

                        var sql2 = "INSERT INTO invoicelines (id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid )" +
                            " VALUES (@Id, @Value, @Description, @Fundcode, @mainaccount, @schemecode,  @marketingyear, @deliverybodycode, @invoicerequestid)";

                        await cn.ExecuteAsync(sql2, bulkUploadApDataset.BulkUploadDetailLines);

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

        public async Task<string> GetDetailLineDescripion(string query, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                using (var transaction = await cn.BeginTransactionAsync(ct))
                {
                    try
                    {
                        // should only be one of these.
                        // but this query "SELECT * FROM public.lookup_ap_chartofaccounts where code='SOS710/5604C/NE99'"
                        // returns 2 ???
                        var description = await cn.QuerySingleAsync<string>(
                                    "SELECT description FROM lookup_ap_chartofaccounts WHERE code = @query",
                                    new { query });

                        await transaction.CommitAsync(ct);

                        return description;
                    }
                    catch
                    {
                        await transaction.RollbackAsync(ct);
                        throw;
                    }
                }
            }
        }

        public async Task<bool> ConfirmOrReject(BulkUploadConfirmation request, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                if (request.Confirm) 
                {
                    var sql = "UPDATE invoices SET bulkuploadconfirmed = @Confirm, status = @Status WHERE id = @InvoiceId";

                    var res = await cn.ExecuteAsync(sql, request);

                    return res == 1;
                }
                else
                {
                    // delete everything...
                    using (var transaction = await cn.BeginTransactionAsync(ct))
                    {
                        try
                        {
                            // get all invoicerequest ids
                            var invoiceRequestIds = await cn.QueryAsync<string>(
                                "SELECT invoicerequestid FROM invoicerequests WHERE invoiceid = @InvoiceId",
                                new { request.InvoiceId },
                                transaction: transaction);

                            // for each invoiceRequestId, delete all invoice lines
                            foreach (string invoiceRequestId in invoiceRequestIds)
                            {
                                await cn.ExecuteAsync(
                                        "DELETE FROM invoicelines WHERE invoicerequestid = @invoiceRequestId",
                                        new { invoiceRequestId },
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
                                    new { request.InvoiceId },
                                    transaction: transaction);

                            await transaction.CommitAsync(ct);

                            return false;
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
}
