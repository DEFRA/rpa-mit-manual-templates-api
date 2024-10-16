using System.Data;
using System.Diagnostics.CodeAnalysis;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsArEndpoint : Endpoint<BulkUploadsArRequest, Response>
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IEmailService _iEmailService;
        private readonly IArImporterService _iArImporterService;
        private readonly IValidationService _iValidationService;
        private readonly ILogger<AddBulkUploadsArEndpoint> _logger;

        public AddBulkUploadsArEndpoint(
            ILogger<AddBulkUploadsArEndpoint> logger,
            IBulkUploadRepo iBulkUploadRepo,        
            IArImporterService iArImporterService,
            IValidationService iValidationService,
            IEmailService iEmailService)
        {
            _logger = logger;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iArImporterService = iArImporterService;
            _iValidationService = iValidationService;
            _iEmailService = iEmailService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/addar");
            AllowFileUploads();
        }

        public override async Task HandleAsync(BulkUploadsArRequest r, CancellationToken ct)
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

                        if (tables?["AR"]?.Rows.Count > 4)
                        {
                            var bulkUploadArDataset = await _iArImporterService.ImportARData(tables["AR"]!, r.Org, ct);

                            var isValid = await _iValidationService.ArBulkUploadIsValid(bulkUploadArDataset, r.Org, ct);

                            if (!isValid)
                            {
                                response.Message = "Invalid data";
                                ThrowError("The supplied data are invalid!");
                            }

                            bulkUploadArDataset.BulkUploadInvoice!.CreatedBy = userEmail;

                            if (await _iBulkUploadRepo.AddArBulkUpload(bulkUploadArDataset, ct))
                            {
                                // email the originator that their file has been successfully uploaded.
                                await _iEmailService.EmailBulkUploadSuccess(userEmail, fileName, bulkUploadArDataset.BulkUploadInvoice.Id, ct);

                                response.BulkUploadArDataset = bulkUploadArDataset;
                            }
                        }
                        else
                        {
                            // No data
                            response.Message = "No recognisable data";
                            await SendAsync(response, 400, cancellation: ct);
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