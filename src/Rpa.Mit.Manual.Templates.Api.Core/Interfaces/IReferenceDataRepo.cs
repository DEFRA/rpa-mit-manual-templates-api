using Rpa.Mit.Manual.Templates.Api.Core.Entities;

namespace Rpa.Mit.Manual.Templates.Api.Core.Interfaces
{
    public interface IReferenceDataRepo
    {
        /// <summary>
        /// gets all our lookup reference data and has it cached
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<ReferenceData> GetAllReferenceData(CancellationToken ct);

        public Task<IEnumerable<SchemeType>> GetSchemeTypeReferenceData(CancellationToken ct);

        public Task<IEnumerable<PaymentType>> GetPaymentTypeReferenceData(CancellationToken ct);
    }
}
