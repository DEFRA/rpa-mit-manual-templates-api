using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// email out to the various relevant approvers that there is a new invoice awaiting approval/rejection
        /// </summary>
        /// <param name="approvers"></param>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> EmailApprovers(IEnumerable<Approver> approvers, Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// emails an invoice creator to inform them that their invoice has been rejected during the approval process.
        /// </summary>
        /// <param name="invoiceCreatorEmail"></param>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> EmailInvoiceRejection(string invoiceCreatorEmail, Guid invoiceId, CancellationToken ct);
    }
}
