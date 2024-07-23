using System.Data;

using BulkUploads.AddAp;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    public class ApImporterService : IApImporterService
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;

        public ApImporterService(IBulkUploadRepo iBulkUploadRepo)
        {
            _iBulkUploadRepo = iBulkUploadRepo;
        }

        public async Task<BulkUploadApDataset> ImportAPData(DataTable data, CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            BulkUploadApDataset bulkUploadApDataset = new BulkUploadApDataset();

            var i = 0;

            foreach (DataRow row in data.Rows)
            {
                i++;
                if (i < 4)
                    continue;

                if (i == 4)
                {
                    // build the invoice
                    bulkUploadApDataset.Invoice = await CreateNewInvoice(row);
                }

                if (!string.IsNullOrEmpty(row[2].ToString()))
                {
                    var bulkUploadHeaderLine = new BulkUploadApHeaderLine
                    {
                        InvoiceId = bulkUploadApDataset.Invoice.Id,
                        InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                        ClaimReferenceNumber = row[2].ToString(),
                        ClaimReference = row[3].ToString(),
                        Currency = row[6].ToString(),
                        MarketingYear = row[24].ToString(),
                        Frn = row[4].ToString(),
                        Description = row[7].ToString()
                    };

                    bulkUploadApDataset.BulkuploadHeaderLines.Add(bulkUploadHeaderLine);

                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();

                    var bulkUploadDetailLine = new BulkUploadApDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = row[2].ToString() + "_" + row[3].ToString(),
                        Value = decimal.Parse(row[19].ToString()),
                        FundCode = row[21].ToString(),
                        MainAccount = row[22].ToString(),
                        SchemeCode = row[23].ToString(),
                        MarketingYear = row[24].ToString(),
                        DeliveryBodyCode = row[25].ToString(),
                        Description = await _iBulkUploadRepo.GetDetailLineDescripion(descriptionQuery, ct)
                    };

                    bulkUploadApDataset.BulkUploadDetailLines.Add(bulkUploadDetailLine);
                }
                else if (!string.IsNullOrEmpty(row[19].ToString()))
                {
                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();

                    var bulkUploadDetailLine = new BulkUploadApDetailLine
                    {
                        Id = Guid.NewGuid(),
                        InvoiceRequestId = row[16].ToString(),
                        Value = decimal.Parse(row[19].ToString()),
                        FundCode = row[21].ToString(),
                        MainAccount = row[22].ToString(),
                        SchemeCode = row[23].ToString(),
                        MarketingYear = row[24].ToString(),
                        DeliveryBodyCode = row[25].ToString(),
                        Description = await _iBulkUploadRepo.GetDetailLineDescripion(descriptionQuery, ct)
                    };

                    bulkUploadApDataset.BulkUploadDetailLines.Add(bulkUploadDetailLine);
                }
            }

            return bulkUploadApDataset;
        }

        private async Task<Invoice> CreateNewInvoice(DataRow row)
        {
            return new Invoice
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                CreatedBy = "aylmer.carson",
                AccountType = "AP",
                SchemeType = row[23].ToString(),
                DeliveryBody = row[25].ToString()
            };
        }
    }
}
