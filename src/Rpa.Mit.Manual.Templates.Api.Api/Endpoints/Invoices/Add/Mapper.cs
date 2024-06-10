using Invoices.Add;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Enums;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Invoices.Add
{
    sealed class InvoiceMapper : Mapper<AddInvoiceRequest, AddInvoiceResponse, Invoice>
    {
        public override Invoice ToEntity(AddInvoiceRequest r) => new()
        {
            AccountType = r.AccountType,
            DeliveryBody = r.DeliveryBody,
            SecondaryQuestion = r.SecondaryQuestion,
            SchemeType = r.SchemeType,
            Value = 0.00M,
            Status = InvoiceStatuses.New
        };
    }
}
