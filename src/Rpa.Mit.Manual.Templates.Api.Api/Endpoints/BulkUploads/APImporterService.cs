using System.Data;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads
{
    public class ApImporterService : IApImporterService
    {
        public async Task<bool> ImportAPData(DataTable data, CancellationToken ct)
        {
            // row 0, col 1 and row 0, col 16 have the 2 titles
            // row 1 is placeholder/empty
            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
            // row 3 = start of data

            foreach (DataRow row in data.Rows)
            {
                var bulkUploadHeaderLine = new BulkUploadHeaderLine
                {
                    InvoiceId = row[1].ToString(),
                    ClaimReferenceNumber = row[2].ToString(),
                    ClaimReference = row[3].ToString(),
                    CustomerId = row[4].ToString(),
                    TotalAmount = row[5].ToString(),
                    PreferredCurrency = row[6].ToString(),
                    Description = row[7].ToString()
                };

            }

            return true;
        }
    }
}
