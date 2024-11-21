

using System.Data;
using System.Diagnostics.CodeAnalysis;

using Dapper;

using Microsoft.Extensions.Options;

using Npgsql;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    [ExcludeFromCodeCoverage]
    public class ApproversRepo : BaseData, IApproversRepo
    {
        public ApproversRepo(IOptions<PostGres> options) : base(options)
        { }

        /// <summary>
        /// this really ought to use a stored proc.
        /// gets a list of valid approvers for a given invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetApproversForInvoice(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var approverRequirements = await cn.QuerySingleAsync<ApproverRequirements>("SELECT schemetype,deliverybody FROM invoices where id = @invoiceId", new { invoiceId });

                var approvers = await cn.QueryAsync<string>("SELECT email FROM lookup_approvers where schemetype = @schemetype and deliverybody = @deliverybody",
                    new { 
                        schemetype = approverRequirements.SchemeType, 
                        deliverybody = approverRequirements.DeliveryBody 
                    });

                return approvers;
            }
        }

        private sealed class ApproverRequirements
        {
            public string SchemeType { get; set; } = string.Empty;
            public string DeliveryBody { get; set; } = string.Empty;
        }
    }
}
