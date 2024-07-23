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

        public IEnumerable<InvoiceRequest> InvoiceRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();

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

        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsGBP => InvoiceRequests.Where(x => x.Currency == "GBP").Sum(x => x.Value);
        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsEUR => InvoiceRequests.Where(x => x.Currency == "EUR").Sum(x => x.Value);
        //[JsonIgnore]
        //public bool CanBeSentForApproval => Status == InvoiceStatuses.New && InvoiceRequests.All(x => x.Value != 0 && x.InvoiceLines.Any());
    }
}
