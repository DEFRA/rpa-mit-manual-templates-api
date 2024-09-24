
using System.Data;
using System.Diagnostics.CodeAnalysis;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsApEndpoint : Endpoint<BulkUploadsApRequest, Response>
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IApImporterService _iApImporterService;
        private readonly IEmailService _iEmailService;
        private readonly ILogger<AddBulkUploadsApEndpoint> _logger;

        public AddBulkUploadsApEndpoint(
            IEmailService iEmailService,    
            ILogger<AddBulkUploadsApEndpoint> logger,
            IBulkUploadRepo iBulkUploadRepo,
            IApImporterService iApImporterService)
        {
            _logger = logger;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iEmailService = iEmailService;
            _iApImporterService = iApImporterService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/addap");
            AllowFileUploads();
        }

        public override async Task HandleAsync(BulkUploadsApRequest r, CancellationToken ct)
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
                        var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        DataTableCollection dataTables = dataSet.Tables;

                        if (null == dataTables)
                        {
                            // No data, return
                            await SendNoContentAsync(cancellation: ct);
                        }
                    
                        if (dataTables?["AP"]?.Rows.Count > 4)
                        {
                            // dealing with AP data
                            var bulkUploadApDataset = await _iApImporterService.ImportAPData(dataTables["AP"]!, ct);

                            bulkUploadApDataset.BulkUploadInvoice!.CreatedBy = userEmail;

                            if (await _iBulkUploadRepo.AddApBulkUpload(bulkUploadApDataset, ct))
                            {
                                // email the originator that their file has been successfully uploaded.
                                await _iEmailService.EmailBulkUploadSuccess(userEmail, fileName, bulkUploadApDataset.BulkUploadInvoice.Id, ct);

                                response.BulkUploadApDataset = bulkUploadApDataset;
                            }
                        }
                        else
                        {
                            // No data
                            response.Message = "No recognisable data";
                        }

                        await SendAsync(response, 200, cancellation: ct);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 500, CancellationToken.None);
            }
        }
    }
}