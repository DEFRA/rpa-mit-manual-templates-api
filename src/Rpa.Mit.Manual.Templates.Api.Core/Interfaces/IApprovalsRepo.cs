
using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApprovalsRepo
    {
        /// <summary>
        /// clears an invoice for sending to the payment hub.
        /// An invoice will be sent there immediately once this approval happens.
        /// </summary>
        /// <param name="invoiceApproval"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ApproveInvoice(InvoiceApproval invoiceApproval, CancellationToken ct);

        /// <summary>
        /// gets a list of all invoices which have the user as the selected approver
        /// </summary>
        /// <param name="approverEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<Invoice>> GetMyApprovals(string approverEmail, CancellationToken ct);

        /// <summary>
        /// gets a single invoice for approver. needs to also check that the person making the request
        /// is one of the people selected as approver.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="approverEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Invoice> GetInvoiceForApproval(Guid invoiceId, string approverEmail, CancellationToken ct);
    }
}
