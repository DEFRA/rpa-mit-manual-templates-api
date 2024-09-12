

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

        /// <summary>
        /// this really ought to use a stored proc.
        /// gets a list of valid approvers for a given invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Approver>> GetApproversForInvoice(Guid invoiceId, CancellationToken ct)
        {
            using (var cn = new NpgsqlConnection(await DbConn()))
            {
                if (cn.State != ConnectionState.Open)
                    await cn.OpenAsync(ct);

                var approverRequirements = await cn.QuerySingleAsync<ApproverRequirements>("SELECT schemetype,deliverybody FROM invoices where id = @invoiceId", new { invoiceId });

                var approvers = await cn.QueryAsync<Approver>("SELECT id,name,email FROM lookup_approvers la JOIN lookup_approvers_scopes las ON la.id = las.approverid where schemetype = @schemetype and deliverybody = @deliverybody",
                    new { 
                        schemetype = approverRequirements.SchemeType, 
                        deliverybody = approverRequirements.DeliveryBody 
                    });

                List<SelectedApprover> selectedApprovers = [];

                foreach (var approver in approvers)
                {
                    selectedApprovers.Add(new SelectedApprover
                    {
                        InvoiceId = invoiceId,
                        ApproverId = approver.Id,
                    });
                }

                //stick these in the db
                var sql = "INSERT INTO invoices_approvers (invoiceid, approverid) VALUES (@invoiceid, @approverid)";

                await cn.ExecuteAsync(sql, selectedApprovers);

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
