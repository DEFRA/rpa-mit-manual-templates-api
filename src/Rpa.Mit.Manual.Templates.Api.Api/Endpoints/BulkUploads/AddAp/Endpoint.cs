
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
                            ThrowError("No data!");
                        }
                    
                        if (dataTables["AP"]?.Rows.Count > 4)
                        {
                            // import into our class structure
                            var importResult = await _iApImporterService.ImportAPData(dataTables["AP"]!, r.Org, r.SchemeInvoiceTemplate, ct);

                            if (!string.IsNullOrEmpty(importResult.Error))
                            {
                                response.Message = importResult.Error;
                            }
                            else
                            {
                                importResult.BulkUploadImport.BulkUploadInvoice!.CreatedBy = userEmail;

                                if (await _iBulkUploadRepo.AddApBulkUpload(importResult.BulkUploadImport, ct))
                                {
                                    // email the originator that their file has been successfully uploaded.
                                    await _iEmailService.EmailBulkUploadSuccess(userEmail, fileName, importResult.BulkUploadImport.BulkUploadInvoice.Id, ct);

                                    response.BulkUploadApDataset = importResult.BulkUploadImport;
                                }
                            }
                        }
                        else
                        {
                            // No data
                            ThrowError("No recognisable data!");
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