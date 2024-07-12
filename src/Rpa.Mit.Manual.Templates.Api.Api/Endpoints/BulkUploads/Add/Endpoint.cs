
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
            if (r.File.Length > 0)
            {
                var filename = r.File.FileName.ToLower();
                var file = Files[0];

                await SendStreamAsync(
                    stream: file.OpenReadStream(),
                    fileName: @"c:\test.xlsm",
                    fileLengthBytes: file.Length,
                    contentType: "application/octet-stream");

                return;
            }
            await SendNoContentAsync();
        }
    }
}