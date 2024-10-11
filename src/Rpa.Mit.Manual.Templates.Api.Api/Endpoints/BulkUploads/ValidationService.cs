using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    [ExcludeFromCodeCoverage]
    public class ValidationService : IValidationService
    {
        private readonly IReferenceDataRepo _iReferenceDataRepo;

        public ValidationService(IReferenceDataRepo iReferenceDataRepo)
        {
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public async Task<bool> ApBulkUploadIsValid(BulkUploadApDataset bulkUploadApDataset, string deliveryBody, CancellationToken ct)
        {

            var fundCodeIsValid = await FundCodeIsValid(bulkUploadApDataset.BulkUploadDetailLines, deliveryBody, ct);


            return fundCodeIsValid;
        }

        /// <summary>
        /// iterate the uploaded detailLines. if there is any single one which doesn't contain a relevant fund code, bail.
        /// </summary>
        /// <param name="bulkUploadDetailLines"></param>
        /// <param name="deliveryBody"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<bool> FundCodeIsValid(IEnumerable<BulkUploadApDetailLine> bulkUploadDetailLines, string deliveryBody, CancellationToken ct)
        {
            var isValid = true;

            var fundCodes = await _iReferenceDataRepo.GetFilteredFundcodesReferenceData(deliveryBody, ct);

            foreach (BulkUploadApDetailLine detailLine in bulkUploadDetailLines)
            {
                isValid = fundCodes.Any(p => p.Code.Contains(detailLine.FundCode));

                if (!isValid) { break; }
            }

            return isValid;
        }
    }
}
