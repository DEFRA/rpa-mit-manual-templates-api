using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public sealed class ReferenceData
    {
        public IEnumerable<DeliveryBodyInitial> InitialDeliveryBodies { get; set; } = Enumerable.Empty<DeliveryBodyInitial>();
        public IEnumerable<Organisation> Organisations { get; set; } = Enumerable.Empty<Organisation>();
        public IEnumerable<SchemeInvoiceTemplate> SchemeInvoiceTemplates { get; set; } = Enumerable.Empty<SchemeInvoiceTemplate>();
        public IEnumerable<SchemeInvoiceTemplateSecondaryQuestion> SchemeInvoiceTemplateSecondaryQuestions { get; set; } = Enumerable.Empty<SchemeInvoiceTemplateSecondaryQuestion>();

        public IEnumerable<SchemeType> SchemeTypes { get; set; } = Enumerable.Empty<SchemeType>();
        public IEnumerable<PaymentType> PaymentTypes { get; set; } = Enumerable.Empty<PaymentType>();
        public IEnumerable<SchemeCode> SchemeCodes { get; set; } = Enumerable.Empty<SchemeCode>();

        public IEnumerable<AccountCode> AccountCodes { get; set; } = Enumerable.Empty<AccountCode>();

        public IEnumerable<DeliveryBody> DeliveryBodies { get; set; } = Enumerable.Empty<DeliveryBody>();
        public IEnumerable<DeliveryBodyInitial> DeliveryBodiesInitial { get; set; } = Enumerable.Empty<DeliveryBodyInitial>();

        public IEnumerable<MarketingYear> MarketingYears { get; set; } = Enumerable.Empty<MarketingYear>();
        public IEnumerable<FundCode> FundCodes { get; set; } = Enumerable.Empty<FundCode>();

        public IEnumerable<ChartOfAccounts> ChartOfAccountsAp { get; set; } = Enumerable.Empty<ChartOfAccounts>();
    }
}
