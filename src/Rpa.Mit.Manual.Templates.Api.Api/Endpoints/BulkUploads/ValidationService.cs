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

        //TODO: rework properly
        public async Task<bool> FundCodeIsValid(IEnumerable<FundCode> fundCodes, string fundcode, string org, CancellationToken ct)
        {
            bool result;

            return await Task.Run(() => result = fundCodes.Any(p => p.Code.Contains(fundcode) && p.Org == org));
        }

        public async Task<bool> MainAccountIsValid(IEnumerable<MainAccount> mainAccounts, string mainAccount, string org, CancellationToken ct)
        {
            bool result;

            return await Task.Run(() => result = mainAccounts.Any(p => p.Code == mainAccount &&  p.Org == org));
        }

        public async Task<bool> InvoiceRequestIdHasCorrectLength(string invoiceRequestId, CancellationToken ct)
        {
            bool result;

            return await Task.Run(() => result = invoiceRequestId.Length == 20);
        }

        public async Task<bool> CustomerIdIsValid(string customerId, string org, string deliveryBody, CancellationToken ct)
        {
            int cId = 0;

            if(await Task.Run(() => int.TryParse(customerId, out cId)))
            {
                // If dBody = "SPS" Or dBody = "DF" Or strOrg = "RDT" Then 'SBI or FRN allowed so id length must be integer greater than 8 and less than 11
                if (deliveryBody == "SPS" || deliveryBody == "DF" || org == "RDT")
                {
                    if (cId.ToString().Length > 8 && cId.ToString().Length < 11)
                    {
                        return true;
                    }

                    return false;
                }
                else if(org == "NE") // Vendor ID or FRN is allowed - integer and longer than 5 and shorter than 11 digits
                {
                    if (cId.ToString().Length > 5 && cId.ToString().Length < 11)
                    {
                        return true;
                    }

                    return false;
                }
                else   // Only FRN is allowed -  integer 10 digits long
                {
                    if (cId.ToString().Length == 10)
                    {
                        return true;
                    }

                    return false;
                }
            }
            else
            {
                return false;
            }
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
                                                    IEnumerable<MainAccount> accountsAp, 
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
