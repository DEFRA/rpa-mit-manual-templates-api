using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using Rpa.Mit.Manual.Templates.Api.Core.Enums;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class Invoice
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Payment Type is required")]
        public string PaymentType { get; set; } = default!;

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; } = default!;

        [Required(ErrorMessage = "DeliveryBody is required")]
        public string DeliveryBody { get; set; } = default!;

        public string SecondaryQuestion { get; set; } = default!;

        [Required(ErrorMessage = "Scheme Type is required")]
        public string SchemeType { get; set; } = default!;

        public IEnumerable<InvoiceRequest> PaymentRequests { get; set; } = Enumerable.Empty<InvoiceRequest>();

        /// <summary>
        /// the sum total of any or all of payment requests
        /// </summary>
        public decimal Value { get; set; } = 0.0M;

        public string Status { get; set; } = InvoiceStatuses.New;

        public string Reference { get; set; } = default!;

        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; } = default!;

        [JsonIgnore]
        public decimal TotalValueOfPaymentsGBP => PaymentRequests.Where(x => x.Currency == "GBP").Sum(x => x.Value);
        [JsonIgnore]
        public decimal TotalValueOfPaymentsEUR => PaymentRequests.Where(x => x.Currency == "EUR").Sum(x => x.Value);
        [JsonIgnore]
        public bool CanBeSentForApproval => Status == InvoiceStatuses.New && PaymentRequests.All(x => x.Value != 0 && x.InvoiceLines.Any());

        public string ApprovalGroup
        {
            get
            {
                if (DeliveryBody == "RPA")
                {
                    return SchemeType;
                }
                return DeliveryBody;
            }
        }
    }
}
