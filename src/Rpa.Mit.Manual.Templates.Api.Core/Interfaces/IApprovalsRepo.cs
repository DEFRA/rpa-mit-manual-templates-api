
using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IApprovalsRepo
    {
        /// <summary>
        /// clears an invoice for sending to the payment hub.
        /// An invoice will be sent there once this approval happens.
        /// </summary>
        /// <param name="invoiceApproval"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ApproveInvoice(InvoiceApproval invoiceApproval, CancellationToken ct);
    }
}
