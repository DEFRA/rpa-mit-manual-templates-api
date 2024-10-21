using System.Data;
using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;


namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{

    /// <summary>
    /// Accounts Payable Importer
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApImporterService : IApImporterService
    {
        private readonly IReferenceDataRepo _iReferenceDataRepo;

        public ApImporterService(IReferenceDataRepo iReferenceDataRepo)
        {
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public async Task<BulkUploadApDataset> ImportAPData(DataTable data, CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            BulkUploadApDataset bulkUploadApDataset = new();
            BulkUploadInvoice bulkUploadInvoice = new();
            decimal totalUploadedValue = 0.0M;

            var i = 0;

            // get all our chartofaccounts before we enter the loop
            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsApReferenceData(ct);

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
                    var bulkUploadHeaderLine = new BulkUploadApHeaderLine
                    {
                        Ledger = "AP",
                        InvoiceId = bulkUploadInvoice!.Id,
                        InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                        ClaimReferenceNumber = row[2].ToString()!,
                        ClaimReference = row[3].ToString()!,
                        PaymentType = row[6].ToString()!,
                        Frn = row[4].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        Description = row[7].ToString()!
                    };

                    bulkUploadInvoice.BulkUploadApHeaderLines!.Add(bulkUploadHeaderLine);

                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();

                    var description = chartOfAccounts.First(c => c.Code == descriptionQuery).Description;

                    if (string.IsNullOrEmpty(description))
                    {
                        throw new Exception("Invalid account/scheme/deliverybody combination");
                    }

                    var bulkUploadDetailLine = new BulkUploadApDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = row[17].ToString() + "_" + row[18].ToString(),
                        Value = decimal.Parse(row[19].ToString()!),
                        FundCode = row[21].ToString()!,
                        MainAccount = row[22].ToString()!,
                        SchemeCode = row[23].ToString()!,
                        DeliveryBodyCode = row[25].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        Description = description
                    };

                    // for the databasee
                    bulkUploadApDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                }
                else if (!string.IsNullOrEmpty(row[19].ToString()))
                {
                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();
                    var invoiceRequestId = row[17].ToString() + "_" + row[18].ToString();

                    var description = chartOfAccounts.First(c => c.Code == descriptionQuery).Description;

                    if (string.IsNullOrEmpty(description))
                    {
                        throw new Exception("Invalid account/scheme/deliverybody combination");
                    }

                    var bulkUploadDetailLine = new BulkUploadApDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = invoiceRequestId,
                        Value = decimal.Parse(row[19].ToString()!),
                        FundCode = row[21].ToString()!,
                        SchemeCode = row[23].ToString()!,
                        MainAccount = row[22].ToString()!,
                        MarketingYear = row[24].ToString()!,
                        DeliveryBodyCode = row[25].ToString()!,
                        Description = description
                    };

                    // this for the database
                    bulkUploadApDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                }
            }

            // nest the data for returning json
            foreach (var parent in bulkUploadInvoice.BulkUploadApHeaderLines!)
            {
                parent.BulkUploadApDetailLines = bulkUploadApDataset.BulkUploadDetailLines
                    .Where(c => c.InvoiceRequestId == parent.InvoiceRequestId)
                    .ToList();

                // total up the value of the detail lines for the parent invoice request
                parent.TotalAmount = parent.BulkUploadApDetailLines.Select(c => c.Value).Sum();

                totalUploadedValue += parent.TotalAmount;
            }

            bulkUploadApDataset.InvoiceTotal = totalUploadedValue;
            bulkUploadApDataset.NumberOfInvoices = bulkUploadApDataset.BulkUploadDetailLines.Count;
            bulkUploadApDataset.BulkUploadInvoice = bulkUploadInvoice;

            return bulkUploadApDataset;
        }

        private static async Task<BulkUploadInvoice> CreateNewInvoice(DataRow row)
        {
            var invoice = await Task.FromResult(new BulkUploadInvoice());

            invoice.Id = Guid.NewGuid();
            invoice.Created = DateTime.UtcNow;
            invoice.AccountType = "AP";
            invoice.SchemeType = row[23].ToString()!;
            invoice.DeliveryBody = row[25].ToString()!;

            return invoice;
        }
    }
}
