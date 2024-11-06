using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Schema;

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
        private readonly IValidationService _iValidationService;

        public ApImporterService(IReferenceDataRepo iReferenceDataRepo, IValidationService iValidationService)
        {
            _iReferenceDataRepo = iReferenceDataRepo;
            _iValidationService = iValidationService;
        }

        public async Task<BulkUploadImportResult<BulkUploadApDataset, string>> ImportAPData(DataTable data, CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            BulkUploadApDataset bulkUploadApDataset = new();
            decimal totalUploadedValue = 0.0M;

            StringBuilder errors = new();

            var i = 0;

            // get all our chartofaccounts etc before we enter the loop
            var chartOfAccounts = await _iReferenceDataRepo.GetChartOfAccountsApReferenceData(ct);
            var mainAccounts = await _iReferenceDataRepo.GetApMainAccountsReferenceData(ct);
            var schemeCodes = await _iReferenceDataRepo.GetSchemeCodesReferenceData(ct);
            var deliveryBodies = await _iReferenceDataRepo.GetDeliveryBodiesReferenceData(ct);

            BulkUploadInvoice bulkUploadInvoice = await CreateNewInvoice(data.Rows[4]);

            foreach (DataRow row in data.Rows)
            {
                i++;

                if (i < 4)
                    continue;

                if (!string.IsNullOrEmpty(row[2].ToString()))
                {
                    var bulkUploadHeaderLine = CreateBulkUploadApHeaderLineFromRow(bulkUploadInvoice!.Id, row);

                    bulkUploadInvoice.BulkUploadApHeaderLines!.Add(bulkUploadHeaderLine);

                    var description = _iValidationService.GetChartOfAccountDescription(chartOfAccounts, mainAccounts, schemeCodes, deliveryBodies, row[22].ToString()!, row[23].ToString()!, row[25].ToString()!);

                    if (string.IsNullOrEmpty(description))
                    {
                        errors.AppendFormat("Error in Line {0} Invalid account/scheme/deliverybody combination", i.ToString());
                    }
                    else
                    {
                        var bulkUploadDetailLine = CreateBulkUploadApDetailLineFromRow(row, description);

                        // for the databasee
                        bulkUploadApDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                    }
                }
                else if (!string.IsNullOrEmpty(row[19].ToString()))
                {
                    var description = _iValidationService.GetChartOfAccountDescription(chartOfAccounts, mainAccounts, schemeCodes, deliveryBodies, row[22].ToString()!, row[23].ToString()!, row[25].ToString()!);

                    if (string.IsNullOrEmpty(description))
                    {
                        errors.AppendFormat("Error in Line {0} Invalid account/scheme/deliverybody combination", i.ToString());
                    }
                    else
                    {
                        var bulkUploadDetailLine = CreateBulkUploadApDetailLineFromRow(row, description);

                        // this for the database
                        bulkUploadApDataset.BulkUploadDetailLines!.Add(bulkUploadDetailLine);
                    }
                }
            }

            if (errors.Length > 0)
            {
                return errors.ToString();
            }
            else
            {
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
        }

        private static BulkUploadApDetailLine CreateBulkUploadApDetailLineFromRow(DataRow row, string description)
        {
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

            return bulkUploadDetailLine;
        }

        private static BulkUploadApHeaderLine CreateBulkUploadApHeaderLineFromRow(Guid invoiceId, DataRow row)
        {
            var bulkUploadHeaderLine = new BulkUploadApHeaderLine
            {
                Ledger = "AP",
                InvoiceId = invoiceId,
                InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                ClaimReferenceNumber = row[2].ToString()!,
                ClaimReference = row[3].ToString()!,
                PaymentType = row[6].ToString()!,
                Frn = row[4].ToString()!,
                MarketingYear = row[24].ToString()!,
                Description = row[7].ToString()!
            };

            return bulkUploadHeaderLine;
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
