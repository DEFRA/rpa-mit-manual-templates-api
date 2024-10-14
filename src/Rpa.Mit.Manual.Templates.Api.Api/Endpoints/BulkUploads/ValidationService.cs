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

        #region AP Validation

        public async Task<bool> ApBulkUploadIsValid(BulkUploadApDataset bulkUploadApDataset, string org, CancellationToken ct)
        {

            var fundCodeIsValid = await FundCodeIsValid(bulkUploadApDataset.BulkUploadDetailLines, org, ct);


            return fundCodeIsValid;
        }

        /// <summary>
        /// iterate the uploaded detailLines. if there is any single one which doesn't contain a relevant fund code, bail.
        /// </summary>
        /// <param name="bulkUploadDetailLines"></param>
        /// <param name="deliveryBody"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task<bool> FundCodeIsValid(IEnumerable<BulkUploadApDetailLine> bulkUploadDetailLines, string org, CancellationToken ct)
        {
            var isValid = true;

            var fundCodes = await _iReferenceDataRepo.GetFilteredFundcodes(org, ct);

            foreach (BulkUploadApDetailLine detailLine in bulkUploadDetailLines)
            {
                isValid = fundCodes.Any(p => p.Code.Contains(detailLine.FundCode));

                if (!isValid) { break; }
            }

            return isValid;
        }

        #endregion


        #region AR Validation

        public async Task<bool> ArBulkUploadIsValid(BulkUploadArDataset bulkUploadArDataset, string org, CancellationToken ct)
        {

            var mainAccountIsValid = await MainAccountIsValid(bulkUploadArDataset.BulkUploadDetailLines, org, ct);


            return mainAccountIsValid;
        }

        private async Task<bool> MainAccountIsValid(IEnumerable<BulkUploadApDetailLine> bulkUploadDetailLines, string org, CancellationToken ct)
        {
            var isValid = true;

            var filteredMainAccounts = await _iReferenceDataRepo.GetArMainAccountsFilteredByOrg(org, ct);

            foreach (BulkUploadApDetailLine detailLine in bulkUploadDetailLines)
            {
                isValid = filteredMainAccounts.Any(p => p.Org == org);

                if (!isValid) { break; }
            }

            return isValid;
        }

        #endregion

    }
}
