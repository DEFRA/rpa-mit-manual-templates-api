using System.Data;

using Dapper;

using Npgsql;

namespace TiReport
{
    public static class Data
    {
        internal static IEnumerable<T1Report> GetT1ReportData(string connString, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(connString))
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                var sql = @"SELECT id, value, description, fundcode, mainaccount, schemecode, marketingyear, deliverybodycode, invoicerequestid, debttype FROM invoicelines";

                return cn.Query<T1Report>(sql);
            }
        }
    }
}
