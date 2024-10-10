using System.Data;
using System.Diagnostics.CodeAnalysis;

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

        public ArImporterService(IReferenceDataRepo iReferenceDataRepo)
        {
            _iReferenceDataRepo = iReferenceDataRepo;
        }


        public async Task<BulkUploadArDataset> ImportARData(DataTable data, CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            BulkUploadArDataset bulkUploadArDataset = new();
            BulkUploadInvoice bulkUploadInvoice = new();

            var i = 0;

            // get all our chartofaccounts before we enter the loop
            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsArReferenceData(ct);
            var accountsAr = await _iReferenceDataRepo.GetAccountsArReferenceData(ct);

            foreach (DataRow row in data.Rows)
            {
                i++;
                if (i < 4)
                    continue;

                if (i == 4)
                {
                    // build the invoice
                    bulkUploadInvoice = await CreateNewInvoice(row);
                }

                if (!string.IsNullOrEmpty(row[2].ToString()))
                {
                    var bulkUploadHeaderLine = new BulkUploadArHeaderLine
                    {
                        Ledger = "AR",
                        InvoiceId = bulkUploadInvoice!.Id,
                        InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                        ClaimReferenceNumber = row[2].ToString()!,
                        ClaimReference = row[3].ToString()!,
                        PaymentType = row[6].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        Frn = row[4].ToString()!,
                        Description = row[7].ToString()!,
                        //new
                        OriginalClaimReference = row[8].ToString()!,
                        OriginalAPInvoiceSettlementDate = row[9].ToString()!,
                        EarliestDatePossibleRecovery = row[10].ToString()!,
                        CorrectionReference = row[11].ToString()!,
                    };

                    bulkUploadInvoice.BulkUploadArHeaderLines!.Add(bulkUploadHeaderLine);

                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();
                    var debtTypeQuery = row[23].ToString() + "/" + row[25].ToString();

                    var bulkUploadDetailLine = new BulkUploadArDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = row[17].ToString() + "_" + row[18].ToString(),
                        Value = decimal.Parse(row[19].ToString()!),
                        FundCode = row[21].ToString()!,
                        MainAccount = row[22].ToString()!,
                        SchemeCode = row[23].ToString()!,
                        DeliveryBodyCode = row[25].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        Description = chartOfAccounts.First(c => c.Code == descriptionQuery).Description,
                        DebtType = accountsAr.First(c => c.Code == row[22].ToString() && c.Org == row[23].ToString()).Type//row[26].ToString()!
                    };

                    // for the databasee
                    bulkUploadArDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                }
                else if (!string.IsNullOrEmpty(row[19].ToString()))
                {
                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();
                    var invReqId = row[17].ToString() + "_" + row[18].ToString();

                    var bulkUploadDetailLine = new BulkUploadArDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = invReqId,
                        Value = decimal.Parse(row[19].ToString()!),
                        FundCode = row[21].ToString()!,
                        MainAccount = row[22].ToString()!,
                        SchemeCode = row[23].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        DeliveryBodyCode = row[25].ToString()!,
                        Description = chartOfAccounts.First(c => c.Code == descriptionQuery).Description,
                        DebtType = accountsAr.First(c => c.Code == descriptionQuery).Type//row[26].ToString()!
                    };

                    // this for the database
                    bulkUploadArDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                }
            }

            // nest the data for returning json
            foreach (var parent in bulkUploadInvoice.BulkUploadArHeaderLines!)
            {
                parent.BulkUploadArDetailLines = bulkUploadArDataset.BulkUploadDetailLines
                    .Where(c => c.InvoiceRequestId == parent.InvoiceRequestId)
                    .ToList();

                // total up the value of the detail lines for the parent invoice request
                parent.TotalAmount = parent.BulkUploadArDetailLines.Select(c => c.Value).Sum();
            }

            bulkUploadArDataset.BulkUploadInvoice = bulkUploadInvoice;

            return bulkUploadArDataset;
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
    }
}
