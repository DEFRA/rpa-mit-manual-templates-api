using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Npgsql;

namespace TiReport
{
    [ExcludeFromCodeCoverage]
    public static class Data
    {
        internal static IEnumerable<T1Report> GetT1ReportData(string connString, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(connString))
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                var sql = @"SELECT 
                                il.value, 
                                fundcode,
                                mainaccount, 
                                schemecode, 
                                deliverybodycode, 
                                il.invoicerequestid, 
                                debttype, 
                                currency, 
                                frn,
                                originalclaimreference AS legacyid,
                                paymenthubdateprocessed AS settlementdate,
                                paymenthuberror AS reason,
                                createdby AS requester,
                                approveremail AS approver
                                FROM invoices i 
                                LEFT JOIN invoicerequests ir ON i.id = ir.invoiceid
                                LEFT JOIN  invoicelines il ON il.invoicerequestid = ir.invoicerequestid";
                

                return cn.Query<T1Report>(sql);
            }
        }
    }
}
