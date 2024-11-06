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

        public async Task<bool> FundCodeIsValid(IEnumerable<FundCode> fundCodes, string fundcode, string org, CancellationToken ct)
        {
            await Task.CompletedTask;

            return fundCodes.Any(p => p.Code.Contains(fundcode));
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

            var mainAccounts = await _iReferenceDataRepo.GetArMainAccountsReferenceData(ct);

            foreach (BulkUploadApDetailLine detailLine in bulkUploadDetailLines)
            {
                isValid = mainAccounts.Any(p => p.Org == org);

                if (!isValid) { break; }
            }

            return isValid;
        }

        #endregion

        #region Chart Of Accounts Validation


        public string? GetChartOfAccountDescription(
                                                    IEnumerable<ChartOfAccounts> chartOfAccounts, 
                                                    IEnumerable<AccountAr> accountsAp, 
                                                    IEnumerable<SchemeType> schemeTypes, 
                                                    IEnumerable<DeliveryBody> deliveryBodies, 
                                                    string mainAccount, 
                                                    string schemeCode, 
                                                    string deliveryBodyCode)
        {
            var descriptionQuery = mainAccount + "/" + schemeCode + "/" + deliveryBodyCode;

            var chartOfAccount = chartOfAccounts.FirstOrDefault(c => c.Code == descriptionQuery);
            if (chartOfAccount != null)
            {
                return chartOfAccount.Description;
            }
            else
            {
                var mainAccounto = accountsAp.FirstOrDefault(c => c.Code == mainAccount);
                if (mainAccounto == null) return null;
                var macDesc = mainAccounto.Description;

                var scsq = schemeTypes.FirstOrDefault(c => c.Code == schemeCode);
                if (scsq == null) return null;
                var scsDesc = scsq.Description;

                var dbsd = deliveryBodies.FirstOrDefault(c => c.Code == deliveryBodyCode);
                if (dbsd == null) return null;
                var dbsDesc = dbsd.Description;

                return macDesc + "/" + scsDesc + "/" + dbsDesc;
            }
        }

        #endregion

    }
}
