

using Npgsql;
using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;
using Microsoft.Extensions.Options;
using Dapper;
using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    [ExcludeFromCodeCoverage]
    public class ApproversRepo : BaseData, IApproversRepo
    {
        public ApproversRepo(IOptions<PostGres> options) : base(options)
        { }

        public async Task<IEnumerable<Approver>> GetApproversForInvoice(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var values = await cn.QuerySingleAsync<ApproverRequirements>("SELECT schemetype,deliverybody FROM invoices where id = @invoiceId", new { invoiceId });


                var approvers = await cn.QueryAsync<Approver>("SELECT id,name,email FROM lookup_approvers where position(schemetype in @schemetype)>0 and position(deliverybody in @deliverybody)>0",
                    new { 
                        schemetype = values.SchemeType, 
                        deliverybody = values.DeliveryBody 
                    });


                return new List<Approver>();
            }
        }

        private sealed class ApproverRequirements
        {
            public string SchemeType { get; set; } = string.Empty;
            public string DeliveryBody { get; set; } = string.Empty;
        }
    }
}
