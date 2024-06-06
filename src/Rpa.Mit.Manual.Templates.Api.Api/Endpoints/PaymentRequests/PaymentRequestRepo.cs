using System.Data;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.PaymentRequests
{
    public class PaymentRequestRepo : BaseData, IPaymentRequestRepo
    {
        public PaymentRequestRepo(IOptions<ConnectionStrings> options) : base(options)
        { }

        public async Task<bool> AddPaymentRequest(PaymentRequest paymentRequest, CancellationToken ct)
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
