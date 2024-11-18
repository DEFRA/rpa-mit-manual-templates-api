using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    /// <summary>
    /// Accounts Receivable Importer
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ArImporterService : IArImporterService
    {
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly IValidationService _iValidationService;

        public ArImporterService(IReferenceDataRepo iReferenceDataRepo, IValidationService iValidationService)
        {
            _iReferenceDataRepo = iReferenceDataRepo;
            _iValidationService = iValidationService;
        }


        public async Task<BulkUploadImportResult<BulkUploadArDataset, string>> ImportARData(DataTable data,
            string org,
            string schemeType,
            CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            BulkUploadArDataset bulkUploadArDataset = new();
            StringBuilder errors = new();

            var i = 0;

            // get all our chartofaccounts etc before we enter the loop
            var marketingYears = await _iReferenceDataRepo.GetMarketingYears(ct);
            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsArReferenceData(ct);
            var mainAccounts = await _iReferenceDataRepo.GetArMainAccountsReferenceData(ct);
            var deliveryBodies = await _iReferenceDataRepo.GetDeliveryBodiesReferenceData(ct);
            var fundCodes = await _iReferenceDataRepo.GetFundcodes(ct);
            var schemeCodes = await _iReferenceDataRepo.GetSchemeCodesReferenceData(ct);

            BulkUploadInvoice bulkUploadInvoice = await CreateNewInvoice(data.Rows[4]);

            foreach (DataRow row in data.Rows)
            {
                i++;

                if (i < 4)
                    continue;

                if (!string.IsNullOrEmpty(row[2].ToString()))
                {
                    var bulkUploadHeaderLine = await CreateArHeaderLineFromRow(bulkUploadInvoice!.Id, row, org, schemeType, i, ct);

                    bulkUploadInvoice.BulkUploadArHeaderLines!.Add(bulkUploadHeaderLine);

                    var description = _iValidationService.GetChartOfAccountDescription(chartOfAccounts, mainAccounts, schemeCodes, deliveryBodies, row[22].ToString()!, row[23].ToString()!, row[25].ToString()!);

                    if (string.IsNullOrEmpty(description))
                    {
                        errors.AppendFormat("Error in Line {0} Invalid account/scheme/deliverybody combination", i.ToString());
                    }
                    else
                    {
                        var bulkUploadDetailLine = await CreateArInvoiceLineFromRow(fundCodes, mainAccounts, marketingYears, row, description, org, i, ct);

                        errors.AppendFormat(bulkUploadDetailLine.Error!.ToString());

                        // for the databasee
                        bulkUploadArDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                    }
                }
                else if (!string.IsNullOrEmpty(row[19].ToString()))
                {
                    var description = _iValidationService.GetChartOfAccountDescription(chartOfAccounts, mainAccounts, schemeCodes, deliveryBodies, row[22].ToString()!, row[23].ToString()!, row[25].ToString()!);

                    if (string.IsNullOrEmpty(description))
                    {
                        errors.AppendFormat("Error in Line {0} Invalid account/scheme/deliverybody combination", i.ToString());
                    }

                    var bulkUploadDetailLine = await CreateArInvoiceLineFromRow(fundCodes, mainAccounts, marketingYears, row, description, org, i, ct);

                    // this for the database
                    bulkUploadArDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                }
            }

            return await ImportResult(errors, bulkUploadInvoice, bulkUploadArDataset);
        }

        private async Task<BulkUploadImportResult<BulkUploadArDataset, string>> ImportResult(StringBuilder errors, BulkUploadInvoice bulkUploadInvoice, BulkUploadArDataset bulkUploadArDataset)
        {
            decimal totalUploadedValue = 0.0M;

            var iter = 0;

            // nest the data for returning json
            foreach (var parent in bulkUploadInvoice.BulkUploadArHeaderLines!)
            {
                iter++;

                parent.BulkUploadArDetailLines = bulkUploadArDataset.BulkUploadDetailLines
                    .Where(c => c.InvoiceRequestId == parent.InvoiceRequestId)
                    .ToList();

                // total up the value of the detail lines for the parent invoice request
                parent.TotalAmount = parent.BulkUploadArDetailLines.Select(c => c.Value).Sum();

                // check that the total is a valid total
                if (!await _iValidationService.InvoiceRequestAmountIsOk(parent.TotalAmount))
                {
                    errors.AppendFormat("Invalid invoice request amount in Line {0}.", iter.ToString());
                }

                totalUploadedValue += parent.TotalAmount;
            }

            bulkUploadArDataset.InvoiceTotal = totalUploadedValue;
            bulkUploadArDataset.NumberOfInvoices = bulkUploadArDataset.BulkUploadDetailLines.Count;
            bulkUploadArDataset.BulkUploadInvoice = bulkUploadInvoice;

            return errors.Length > 0
                ? (BulkUploadImportResult<BulkUploadArDataset, string>)errors.ToString()
                : (BulkUploadImportResult<BulkUploadArDataset, string>)bulkUploadArDataset;
        }

        private static async Task<BulkUploadInvoice> CreateNewInvoice(DataRow row)
        {
            var invoice = await Task.FromResult(new BulkUploadInvoice());

            invoice.Id = Guid.NewGuid();
            invoice.Created = DateTime.UtcNow;
            invoice.AccountType = "AR";
            invoice.SchemeType = row[23].ToString()!;
            invoice.DeliveryBody = row[25].ToString()!;

            return invoice;
        }

        private async Task<BulkUploadArHeaderLine> CreateArHeaderLineFromRow(
                                                                            Guid invoiceId,
                                                                            DataRow row,
                                                                            string org,
                                                                            string schemeInvoiceTemplate,
                                                                            int i,
                                                                            CancellationToken ct)
        {
            var bulkUploadHeaderLine = new BulkUploadArHeaderLine
            {
                Ledger = "AR",
                InvoiceId = invoiceId,
                InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                ClaimReferenceNumber = row[2].ToString()!,
                ClaimReference = row[3].ToString()!,
                PaymentType = row[6].ToString()!,
                Frn = row[4].ToString()!,
                Description = row[7].ToString()!,
                MarketingYear = row[24].ToString()!,
                //new
                OriginalClaimReference = row[8].ToString()!,
                OriginalAPInvoiceSettlementDate = row[9].ToString()!,
                EarliestDatePossibleRecovery = row[10].ToString()!,
                CorrectionReference = row[11].ToString()!,
                Error = new StringBuilder()
            };


            if (!await _iValidationService.InvoiceRequestIdHasCorrectLength(bulkUploadHeaderLine.InvoiceRequestId, ct))
            {
                bulkUploadHeaderLine.Error.AppendFormat("Invoice Request Id has incorrect length in Line {0}", i.ToString());
            }

            if (!await _iValidationService.CustomerIdIsValid(bulkUploadHeaderLine.Frn, org, schemeInvoiceTemplate, ct))
            {
                bulkUploadHeaderLine.Error.AppendFormat("Invalid customer id in Line {0}", i.ToString());
            }

            return bulkUploadHeaderLine;
        }

        private async Task<BulkUploadArDetailLine> CreateArInvoiceLineFromRow(
                                                                            IEnumerable<FundCode> fundCodes,
                                                                            IEnumerable<MainAccount> mainAccounts,
                                                                            IEnumerable<MarketingYear> marketingYears,
                                                                            DataRow row,
                                                                            string description,
                                                                            string org,
                                                                            int i,
                                                                            CancellationToken ct)
        {
            var debtType = GetDebtType(mainAccounts, org, row[22].ToString()!);

            var bulkUploadDetailLine = new BulkUploadArDetailLine
            {
                Id = Guid.NewGuid(),
                InvoiceRequestId = row[17].ToString() + "_" + row[18].ToString(),
                Value = decimal.Parse(row[19].ToString()!),
                MainAccount = row[22].ToString()!,
                FundCode = row[21].ToString()!,
                SchemeCode = row[23].ToString()!,
                DeliveryBodyCode = row[25].ToString()!,
                MarketingYear = row[24].ToString()!,
                Description = description,
                DebtType = debtType,
                Error = new StringBuilder()
            };

            if (debtType == string.Empty)
            {
                bulkUploadDetailLine.Error.AppendFormat("Error retrieving debt type in Line {0}", i.ToString());
            }

            if (!await _iValidationService.MarketingYearIsValid(marketingYears, bulkUploadDetailLine.MarketingYear, ct))
            {
                bulkUploadDetailLine.Error.AppendFormat("Invalid marketing year in Line {0}", i.ToString());
            }

            if (!await _iValidationService.FundCodeIsValid(fundCodes, bulkUploadDetailLine.FundCode, bulkUploadDetailLine.MainAccount, ct))
            {
                bulkUploadDetailLine.Error.AppendFormat("Invalid fund code in Line {0}", i.ToString());
            }

            if (!await _iValidationService.MainAccountIsValid(mainAccounts, bulkUploadDetailLine.MainAccount, org, ct))
            {
                bulkUploadDetailLine.Error.AppendFormat("Invalid main account in Line {0}", i.ToString());
            }

            return bulkUploadDetailLine;
        }

        /// <summary>
        /// get the single debt type for given org and ar main account
        /// </summary>
        /// <param name="org"></param>
        /// <param name="mainAccount"></param>
        /// <returns></returns>
        private static string GetDebtType(IEnumerable<MainAccount> mainAccounts, string org, string mainAccount)
        {
            var debtType = mainAccounts.FirstOrDefault(x => x.Org == org && x.Code == mainAccount);

            if (debtType == null)
            {
                return string.Empty;
            }
            else
            {
                return debtType.Type!;
            }
        }
                                                //        => mainAccounts.Single(x => x.Org == org && x.Code == mainAccount).Type!;
    }
}
