
using System.Data;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAp
{
    internal sealed class AddBulkUploadsEndpoint : Endpoint<Request, Response>
    {
        private readonly IInvoiceLineRepo _iInvoiceLineRepo;
        private readonly IApImporterService _iApImporterService;
        private readonly ILogger<AddBulkUploadsEndpoint> _logger;

        public AddBulkUploadsEndpoint(
            ILogger<AddBulkUploadsEndpoint> logger,
            IApImporterService iApImporterService,
            IInvoiceLineRepo iInvoiceLineRepo)
        {
            _logger = logger;
            _iApImporterService = iApImporterService;
            _iInvoiceLineRepo = iInvoiceLineRepo;
        }

        public override void Configure()
        {
            Post("/bulkuploads/add");
            AllowFileUploads();
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken ct)
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
                                await _iApImporterService.ImportAPData(tables["AP"], ct);
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