﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Invoices.Add
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceRequest
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

        //public IEnumerable<PaymentRequest> PaymentRequests { get; set; } = Enumerable.Empty<PaymentRequest>();

        /// <summary>
        /// the sum total of any or all of payment requests
        /// </summary>
        //public decimal Value { get; set; } = 0.0M;

        //public string Status { get; private set; } = InvoiceStatuses.New;

        //public string Reference { get; set; } = default!;


        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsGBP => PaymentRequests.Where(x => x.Currency == "GBP").Sum(x => x.Value);
        //[JsonIgnore]
        //public decimal TotalValueOfPaymentsEUR => PaymentRequests.Where(x => x.Currency == "EUR").Sum(x => x.Value);
        //[JsonIgnore]
        //public bool CanBeSentForApproval => Status == InvoiceStatuses.New && PaymentRequests.All(x => x.Value != 0 && x.InvoiceLines.Any());

        //public string ApprovalGroup
        //{
        //    get
        //    {
        //        if (DeliveryBody == "RPA")
        //        {
        //            return SchemeType;
        //        }
        //        return DeliveryBody;
        //    }
        //}
    }

    [ExcludeFromCodeCoverage]
    internal sealed class AddInvoiceResponse
    {
        public string Message { get; set; } = string.Empty;

        public Invoice? Invoice { get; set; }
    }
}
