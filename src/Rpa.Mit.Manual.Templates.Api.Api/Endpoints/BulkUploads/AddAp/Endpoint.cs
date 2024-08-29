
using System.Data;
using System.Diagnostics.CodeAnalysis;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsEndpoint : Endpoint<BulkUploadsRequest, Response>
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IEmailService _iEmailService;
        private readonly IApImporterService _iApImporterService;
        private readonly ILogger<AddBulkUploadsEndpoint> _logger;

        public AddBulkUploadsEndpoint(
            IEmailService iEmailService,    
            IBulkUploadRepo iBulkUploadRepo,
            ILogger<AddBulkUploadsEndpoint> logger,
            IApImporterService iApImporterService)
        {
            _logger = logger;
            _iEmailService = iEmailService;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iApImporterService = iApImporterService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/add");
            AllowFileUploads();
        }

        public override async Task HandleAsync(BulkUploadsRequest r, CancellationToken ct)
        {
            Response response = new Response();
            string fileName = r.File.FileName;
            var userEmail = User.Identity?.Name!;

            try
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
                            await SendNoContentAsync(cancellation: ct);
                        }
                    
                        if (tables?["AP"]?.Rows.Count > 4)
                        {
                            // dealing with AP data
                            var bulkUploadApDataset = await _iApImporterService.ImportAPData(tables["AP"]!, ct);

                            bulkUploadApDataset.BulkUploadInvoice!.CreatedBy = userEmail;

                            if (await _iBulkUploadRepo.AddApBulkUpload(bulkUploadApDataset, ct))
                            {
                                // email the originator that their file has been successfully uploaded.
                                await _iEmailService.EmailBulkUploadSuccess(userEmail, fileName, bulkUploadApDataset.BulkUploadInvoice.Id, ct);

                                response.BulkUploadApDataset = bulkUploadApDataset;
                            }
                        }
                        else if (tables?["AR"]?.Rows.Count > 4)
                        {
                            // dealing with AR data
                            response.Message = "Currently not able to handle AR data";
                        }
                        else
                        {
                            // No data
                            response.Message = "No recognisable requirement";
                        }

                        await SendAsync(response, 200, cancellation: ct);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }
    }
}