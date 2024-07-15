
using System.Data;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads
{
    internal sealed class AddBulkUploadsEndpoint : Endpoint<Request, Response>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly ILogger<AddBulkUploadsEndpoint> _logger;

        public AddBulkUploadsEndpoint(
            ILogger<AddBulkUploadsEndpoint> logger,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Post("/bulkuploads/add");
            AllowFileUploads();
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            try
            {
                if (r.File.Length > 0)
                {
                    using (var stream = r.File.OpenReadStream())
                    {

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            DataTableCollection tables = result.Tables;

                            if (null == tables)
                            {
                                // No data, return
                                await SendNoContentAsync();
                            }

                            if (tables?["AP"]?.Rows.Count > 4)
                            {
                                // dealing with AP data
                                DataTable resultTable = tables["AP"];
                            }
                            else if (tables?["AR"]?.Rows.Count > 4)
                            {
                                // dealing with AR data
                            }
                            else
                            {
                                // No data, return
                                await SendNoContentAsync();
                            }

                            // row 0, col 1 and row 0, col 16 have the 2 titles
                            // row 1 is placeholder/empty
                            // row 2, cols 1-8 and row 2, col 16-28 have the data headers
                            // row 3 = start of data

                            foreach (DataRow row in resultTable.Rows)
                            {
                                Console.Write($"{row[1]}, {row[2]}, {row[3]} ");
                                Console.WriteLine();
                            }
                        }
                    }

                    //var filename = r.File.FileName.ToLower();

                    //await SendStreamAsync(
                    //    stream: r.File.OpenReadStream(),
                    //    fileName: @"c:\temp\test.xlsm",
                    //    fileLengthBytes: r.File.Length,
                    //    contentType: "application/octet-stream");

                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);


                await SendNoContentAsync();
            }

        }
    }
}