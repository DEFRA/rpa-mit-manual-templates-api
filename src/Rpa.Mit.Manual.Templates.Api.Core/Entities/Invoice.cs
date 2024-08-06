using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Enums;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class Invoice
    {
        public Guid Id { get; set; }

        public string PaymentType { get; set; } = default!;

        public string AccountType { get; set; } = default!;

        public string DeliveryBody { get; set; } = default!;

        public string SecondaryQuestion { get; set; } = default!;

        public string SchemeType { get; set; } = default!;

        /// <summary>
        /// the sum total of any or all of payment requests
        /// </summary>
        public decimal Value { get; set; } = 0.0M;

        public string Status { get; set; } = InvoiceStatuses.New;

        public string Reference { get; set; } = default!;

        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; } = default!;

        public string ApprovalGroup
        {
            get
            {
                return DeliveryBody == "RPA" ? SchemeType : DeliveryBody;
            }
        }

        public IEnumerable<InvoiceRequest> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();
    }
}
