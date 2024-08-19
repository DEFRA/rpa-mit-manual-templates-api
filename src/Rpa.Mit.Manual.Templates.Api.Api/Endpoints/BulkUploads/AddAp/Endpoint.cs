
using System.Data;
using System.Diagnostics.CodeAnalysis;

using ExcelDataReader;

using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAp
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsEndpoint : Endpoint<BulkUploadsRequest, Response>
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IApImporterService _iApImporterService;
        private readonly ILogger<AddBulkUploadsEndpoint> _logger;

        public AddBulkUploadsEndpoint(
            IBulkUploadRepo iBulkUploadRepo,
            ILogger<AddBulkUploadsEndpoint> logger,
            IApImporterService iApImporterService)
        {
            _logger = logger;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iApImporterService = iApImporterService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/add");
            AllowFileUploads();
            //AllowAnonymous();
        }

        public override async Task HandleAsync(BulkUploadsRequest r, CancellationToken ct)
        {
            Response response = new Response();

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

                            bulkUploadApDataset.BulkUploadInvoice = new BulkUploadInvoice();

                            bulkUploadApDataset.BulkUploadInvoice.CreatedBy = User.Identity?.Name!;

                            if (await _iBulkUploadRepo.AddApBulkUpload(bulkUploadApDataset, ct))
                            {
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

                        await SendAsync(response, cancellation: ct);
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