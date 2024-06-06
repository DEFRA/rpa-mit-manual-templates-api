using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IReferenceDataRepo
    {
        public Task<ReferenceData> GetAllReferenceData();

        public Task<IEnumerable<Organisation>> GetOrganisationsReferenceData();

        public Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData();

        public Task<IEnumerable<PaymentType>> GetPaymentTypeReferenceData();
    }
}
