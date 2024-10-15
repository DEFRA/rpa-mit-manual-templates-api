using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Npgsql;

namespace T2Report
{
    [ExcludeFromCodeCoverage]
    public static class Data
    {
        internal static IEnumerable<T2Report> GetT2ReportData(string connString)
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
                                LEFT JOIN invoicelines il ON il.invoicerequestid = ir.invoicerequestid";


                return cn.Query<T2Report>(sql);
            }
        }
    }
}
