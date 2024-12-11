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

                var sql = @"INSERT INTO Invoices (Id, SchemeType, Reference, Status, CreatedBy, Created, PaymentType, AccountType, DeliveryBody, SecondaryQuestion, ApprovalGroup)
                                VALUES (@Id, @SchemeType, @Reference, @Status, @CreatedBy, @Created, @PaymentType, @AccountType, @DeliveryBody, @SecondaryQuestion, @ApprovalGroup)";

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

                await cn.ExecuteAsync(
                        "DELETE FROM invoices WHERE id = @invoiceId",
                        new { invoiceId });

                return true;
            }
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoices(CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var sql = "SELECT id,schemetype,reference,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices";


                return await cn.QueryAsync<Invoice>(sql);
            }
        }

        public async Task<Invoice> GetInvoiceByInvoiceId(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var invoice = new Invoice();

                var sql = @"
                            SELECT id,schemetype,reference,status,approverid,approveremail,approvedby,approved,createdby, updatedby, created, updated,paymenttype,accounttype,deliverybody FROM invoices WHERE Id = @invoiceid;
                            SELECT value FROM public.invoicelines WHERE invoicerequestid IN (SELECT invoicerequestid FROM invoicerequests WHERE invoiceid=@invoiceid);
                        ";

                var parameters = new { invoiceId };

                using (var res = await cn.QueryMultipleAsync(sql, parameters))
                {
                    invoice = await res.ReadSingleAsync<Invoice>();
                    var values= await res.ReadAsync<decimal>();
                    invoice.Value = values.Sum();
                }

                return invoice;
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

        public async Task<DropdownsResponse> GetDropdowns(DropdownsRequest dropdownsRequest, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var dropdownsResponse = new DropdownsResponse();

                var sql = @"
                        SELECT values FROM lookup_dropdowns where name = @FundNamedRange;
                        SELECT values FROM lookup_dropdowns where name = @AccountNamedRange;
                        SELECT values FROM lookup_dropdowns where name = @SchemeTypeNamedRange;
                        SELECT values FROM lookup_dropdowns where name = @MarketingYearNamedRange;
                        SELECT values FROM lookup_dropdowns where name = @DeliveryBodyNamedRange;
                        ";

                using (var res = await cn.QueryMultipleAsync(sql,  new { 
                                                                        dropdownsRequest.FundNamedRange, 
                                                                        dropdownsRequest.AccountNamedRange,
                                                                        dropdownsRequest.SchemeTypeNamedRange,
                                                                        dropdownsRequest.MarketingYearNamedRange,
                                                                        dropdownsRequest.DeliveryBodyNamedRange
                                                                    }))
                {
                    dropdownsResponse.Funds = await res.ReadSingleAsync<string>();
                    dropdownsResponse.Accounts = await res.ReadSingleAsync<string>();
                    dropdownsResponse.SchemeTypes = await res.ReadSingleAsync<string>();
                    dropdownsResponse.MarketingYears = await res.ReadSingleAsync<string>();
                    dropdownsResponse.DeliveryBodies = await res.ReadSingleAsync<string>();

                    return dropdownsResponse;
                }
            }
        }
    }
}
