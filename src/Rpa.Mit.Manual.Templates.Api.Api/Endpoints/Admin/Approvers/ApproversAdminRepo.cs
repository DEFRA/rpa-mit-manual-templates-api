using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Entities.Admin;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Admin.Approvers
{
    [ExcludeFromCodeCoverage]
    public class ApproversAdminRepo : BaseData, IApproversAdminRepo
    {
        public ApproversAdminRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<AdminApprover> Getlookup_approver(string email, string deliverybody)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string getQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE email = @email, deliverybody = @deliverybody";
                return await db.QuerySingleAsync<AdminApprover>(getQuery, new { email, deliverybody });
            }
        }

        public async Task<IEnumerable<AdminApprover>> FindAlllookup_approvers()
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string findAllQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers";
                var results = await db.QueryAsync<AdminApprover>(findAllQuery);
                return results;
            }
        }

        public async Task<IEnumerable<AdminApprover>> Findlookup_approversByAll(string email, string deliverybody, string schemecode, int? threshold)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string findByAllQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE (@email IS NULL OR email = @email), (@deliverybody IS NULL OR deliverybody = @deliverybody), (@schemecode IS NULL OR schemecode = @schemecode), (@threshold IS NULL OR threshold = @threshold)";
                var results = await db.QueryAsync<AdminApprover>(findByAllQuery, new { email, deliverybody, schemecode, threshold });
                return results;
            }
        }

        public async Task<IEnumerable<AdminApprover>> Findlookup_approversByAny(string email, string deliverybody, string schemecode, int? threshold)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string findByAnyQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE (@email IS NOT NULL AND email = @email), (@deliverybody IS NOT NULL AND deliverybody = @deliverybody), (@schemecode IS NOT NULL AND schemecode = @schemecode), (@threshold IS NOT NULL AND threshold = @threshold)";
                var results = await db.QueryAsync<AdminApprover>(findByAnyQuery, new { email, deliverybody, schemecode, threshold });
                return results;
            }
        }

        public async Task<int> Createlookup_approver(string email, string deliverybody, string schemecode, int? threshold)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string insertQuery = "INSERT INTO lookup_approvers (email, deliverybody, schemecode, threshold) VALUES (@email, @deliverybody, @schemecode, @threshold)";
                var rowsAffected = await db.ExecuteScalarAsync<int>(insertQuery, new { email, deliverybody, schemecode, threshold });
                return rowsAffected;
            }
        }

        public async Task<int> Updatelookup_approver(string email, string deliverybody, string schemecode, int? threshold)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string updateQuery = "UPDATE lookup_approvers SET email = @email, deliverybody = @deliverybody, schemecode = @schemecode, threshold = @threshold WHERE email = @email, deliverybody = @deliverybody";
                var rowsAffected = await db.ExecuteScalarAsync<int>(updateQuery, new { email, deliverybody });
                return rowsAffected;
            }
        }

        public async Task<int> Deletelookup_approver(string email, string deliverybody, string schemecode, int? threshold)
        {
            using (IDbConnection db = new NpgsqlConnection(await DbConn()))
            {
                const string deleteQuery = "DELETE lookup_approvers WHERE email = @email, deliverybody = @deliverybody";
                var rowsAffected = await db.ExecuteScalarAsync<int>(deleteQuery, new { email, deliverybody });
                return rowsAffected;
            }
        }
    }
}
