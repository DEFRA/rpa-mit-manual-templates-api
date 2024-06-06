using System.Data;

using Dapper;

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
                    await cn.OpenAsync(ct);

                var sql = "WWW";

                var res = await cn.ExecuteAsync(sql, ct);

                return res == 1;
            }
        }
    }
}
