using System.Net.Mail;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Entities.Azure;

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

        /// <summary>
        /// emails a person doing a bulk upload that their file has been successfully uploaded into the system.
        /// </summary>
        /// <param name="invoiceCreatorEmail"></param>
        /// <param name="invoiceId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> EmailBulkUploadSuccess(string invoiceCreatorEmail, string filename, Guid invoiceId, CancellationToken ct);

        /// <summary>
        /// sends an email to whoever submitted invoice data that the payment hub has rejected the invoice and raised an error.
        /// </summary>
        /// <param name="invoiceCreatorEmail"></param>
        /// <param name="invoiceRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> EmailPaymentHubError(string invoiceCreatorEmail, PaymentHubResponseRoot invoiceRequest, CancellationToken ct);

        /// <summary>
        ///  emails a report to a specified email box
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="reportName"></param>
        /// <param name="attachment"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> EmailReport(string recipientEmail, string reportName, byte[] attachment, CancellationToken ct);
    }
}
