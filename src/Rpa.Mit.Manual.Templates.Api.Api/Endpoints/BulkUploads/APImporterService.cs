using System.Data;

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

                if (!string.IsNullOrEmpty(row[2].ToString()))
                {
                    var bulkUploadHeaderLine = new BulkUploadApHeaderLine
                    {
                        InvoiceId = row[1].ToString(),
                        ClaimReferenceNumber = row[2].ToString(),
                        ClaimReference = row[3].ToString(),
                        PreferredCurrency = row[6].ToString(),

                        CustomerId = row[4].ToString(),
                        //TotalAmount = row[5].ToString(),
                        Description = row[7].ToString()
                    };

                    bulkUploadApDataset.BulkuploadHeaderLines.Add(bulkUploadHeaderLine);


                    var descriptionQuery = row[22].ToString() + "/" + row[23].ToString() + "/" + row[25].ToString();

                    var bulkUploadDetailLine = new BulkUploadApDetailLine
                    {
                        InvoiceId = row[16].ToString(),
                        ClaimReferenceNumber = row[17].ToString(),
                        ClaimReference = row[18].ToString(),
                        PreferredCurrency = row[20].ToString(),

                        Amount = row[19].ToString(),
                        Fund = row[21].ToString(),
                        MainAccount = row[22].ToString(),
                        Scheme = row[23].ToString(),
                        MarketingYear = row[24].ToString(),
                        DeliveryBodyCode = row[25].ToString(),
                        Description = await _iBulkUploadRepo.GetDetailLineDescripions(descriptionQuery, ct)
                    };

                    bulkUploadApDataset.BulkUploadDetailLines.Add(bulkUploadDetailLine);
                }
            }

            return bulkUploadApDataset;
        }
    }
}
