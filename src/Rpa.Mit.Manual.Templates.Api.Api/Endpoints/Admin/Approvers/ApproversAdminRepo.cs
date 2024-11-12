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

        public async Task<AdminApprover> Get(string email, string deliverybody, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string getQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE email = @email, deliverybody = @deliverybody";
                
                return await cn.QuerySingleAsync<AdminApprover>(getQuery, new { email, deliverybody });
            }
        }

        public async Task<IEnumerable<AdminApprover>> GetAll(CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string findAllQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers";

                var results = await cn.QueryAsync<AdminApprover>(findAllQuery);

                return results;
            }
        }

        public async Task<IEnumerable<AdminApprover>> GetByAll(string? email, string? deliverybody, string? schemecode, int? threshold, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string findByAllQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE (@email IS NULL OR email = @email) AND (@deliverybody IS NULL OR deliverybody = @deliverybody) AND (@schemecode IS NULL OR schemecode = @schemecode) AND (@threshold IS NULL OR threshold = @threshold)";
                var results = await cn.QueryAsync<AdminApprover>(findByAllQuery, new { email, deliverybody, schemecode, threshold });
                return results;
            }
        }

        public async Task<IEnumerable<AdminApprover>> GetByAny(string email, string deliverybody, string schemecode, int? threshold, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string findByAnyQuery = "SELECT email,threshold,deliverybody,schemecode FROM lookup_approvers WHERE (@email IS NOT NULL AND email = @email), (@deliverybody IS NOT NULL AND deliverybody = @deliverybody), (@schemecode IS NOT NULL AND schemecode = @schemecode), (@threshold IS NOT NULL AND threshold = @threshold)";
                
                var results = await cn.QueryAsync<AdminApprover>(findByAnyQuery, new { email, deliverybody, schemecode, threshold });

                return results;
            }
        }

        public async Task<bool> Create(AdminApprover adminApprover, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string insertQuery = "INSERT INTO lookup_approvers (email, deliverybody, schemecode, threshold) VALUES (@Email, @DeliveryBody, @SchemeCode, @Threshold)";
                var rowsAffected = await cn.ExecuteAsync(insertQuery, adminApprover);
                return rowsAffected == 1;
            }
        }

        public async Task<bool> Update(AdminApprover adminApprover, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string updateQuery = "UPDATE lookup_approvers SET schemecode = @schemecode, threshold = @threshold WHERE email = @Email AND Deliverybody = @deliverybody";
                
                var rowsAffected = await cn.ExecuteAsync(updateQuery, adminApprover);

                return rowsAffected == 1;
            }
        }

        public async Task<bool> Delete(string email, string deliverybody, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                const string deleteQuery = "DELETE FROM lookup_approvers WHERE email = @Email AND deliverybody = @Deliverybody";

                var rowsAffected = await cn.ExecuteAsync(deleteQuery, new { email, deliverybody });

                return rowsAffected == 1;
            }
        }
    }
}
