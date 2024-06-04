using Npgsql;
using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Microsoft.Extensions.Options;
using Dapper;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices
{
    public class InvoiceDataRepo : BaseData, IInvoiceDataRepo
    {
        public InvoiceDataRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> AddInvoice(Invoice invoice)
        {
            using (var cn = new NpgsqlConnection(DbConn))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync();

                using (var trans = await cn.BeginTransactionAsync())
                {

                    //var affectedRows1 = await cn.ExecuteAsync(sql, new { CustomerName = "Mark" }, transaction: trans);

                    await trans.CommitAsync();
                }
            }

            return true;
        }
    }
}
