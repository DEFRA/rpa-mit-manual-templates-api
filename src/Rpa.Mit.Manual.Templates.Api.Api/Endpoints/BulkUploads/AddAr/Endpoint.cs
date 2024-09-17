using System.Diagnostics.CodeAnalysis;

using BulkUploads.AddAp;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploads.AddAr
{
    [ExcludeFromCodeCoverage]
    internal sealed class AddBulkUploadsArEndpoint : Endpoint<BulkUploadsArRequest, Response>
    {
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IEmailService _iEmailService;
        private readonly IArImporterService _iArImporterService;
        private readonly ILogger<AddBulkUploadsApEndpoint> _logger;

        public AddBulkUploadsArEndpoint(
            IEmailService iEmailService,
            IBulkUploadRepo iBulkUploadRepo,
            ILogger<AddBulkUploadsArEndpoint> logger,
            IArImporterService iArImporterService)
        {
            _logger = logger;
            _iEmailService = iEmailService;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iArImporterService = _ArImporterService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/addar");
            AllowFileUploads();
        }

        public override async Task HandleAsync(BulkUploadsArRequest r, CancellationToken c)
        {
            await SendAsync(new Response());
        }
    }
}