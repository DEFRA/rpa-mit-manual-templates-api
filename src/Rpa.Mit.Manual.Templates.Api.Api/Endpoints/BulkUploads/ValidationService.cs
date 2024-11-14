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
        public async Task<bool> FundCodeIsValid(IEnumerable<FundCode> fundCodes, string fundcode, string mainAccount, CancellationToken ct)
        {
            if ((mainAccount == "SOS228" || mainAccount == "SOS229") && fundcode != "EXQ00")
            {
                return await Task.Run(() => false);
            }
            else
            {
                return await Task.Run(() => fundCodes.Any(p => p.Code.Contains(fundcode)));
            }
        }

        public async Task<bool> MainAccountIsValid(IEnumerable<MainAccount> mainAccounts, string mainAccount, string org, CancellationToken ct)
            => await Task.Run(() => mainAccounts.Any(p => p.Code == mainAccount &&  p.Org == org));

        public async Task<bool> InvoiceRequestIdHasCorrectLength(string invoiceRequestId, CancellationToken ct) 
            => await Task.Run(() => invoiceRequestId.Length == 20);

        public async Task<bool> CustomerIdIsValid(string customerId, string org, string schemeInvoiceTemplate, CancellationToken ct)
        {
            int cId = 0;

            if(await Task.Run(() => int.TryParse(customerId, out cId)))
            {
                // If schemeInvoiceTemplate = "SPS" Or schemeInvoiceTemplate = "DF" Or org = "RDT" Then 'SBI or FRN allowed so id length must be integer greater than 8 and less than 11
                if (schemeInvoiceTemplate == "SPS" || schemeInvoiceTemplate == "DF" || org == "RDT")
                {
                    return cId.ToString().Length > 8 && cId.ToString().Length < 11;
                }

                // if org = "NE" then Vendor ID or FRN is allowed else only FRN is allowed
                return org == "NE" ?  cId.ToString().Length > 5 && cId.ToString().Length < 11: cId.ToString().Length == 10;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> InvoiceRequestAmountIsOk(decimal invoiceRequestAmount) 
            => await Task.Run(() => invoiceRequestAmount > -999999999 && invoiceRequestAmount < 999999999);

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


        public string GetChartOfAccountDescription(
                                                    IEnumerable<ChartOfAccounts> chartOfAccounts, 
                                                    IEnumerable<MainAccount> accountsAp, 
                                                    IEnumerable<SchemeCode> schemeCodes, 
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

                if (mainAccounto == null) return "....";

                var macDesc = mainAccounto.Description;

                var scsq = schemeCodes.FirstOrDefault(c => c.Code == schemeCode);

                if (scsq == null) return "....";

                var scsDesc = scsq.Description;

                var dbsd = deliveryBodies.FirstOrDefault(c => c.Code == deliveryBodyCode);

                if (dbsd == null) return "....";

                var dbsDesc = dbsd.Description;

                return macDesc + "/" + scsDesc + "/" + dbsDesc;
            }
        }

        #endregion

    }
}
