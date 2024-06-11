using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IReferenceDataRepo
    {
        public Task<ReferenceData> GetAllReferenceData(CancellationToken ct);

        public Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData(CancellationToken ct);

        public Task<IEnumerable<PaymentType>> GetPaymentTypeReferenceData(CancellationToken ct);
    }
}
