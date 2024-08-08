using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApproversRepo
    {
        /// <summary>
        /// gets a list of approvers for a given invoice.
        /// this requires the deliverybody and scheme codes from the invoice header
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<Approver>> GetApproversForInvoice(Guid invoiceId, CancellationToken ct);
    }
}
