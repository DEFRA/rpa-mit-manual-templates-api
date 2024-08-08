using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Api.Endpoints.BulkUploads;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace BulkUploadConfirm
{
    [ExcludeFromCodeCoverage]
    internal sealed class BulkUploadConfirmEndpoint : EndpointWithMapping<BulkUploadConfirmRequest, BulkUploadConfirmResponse, BulkUploadConfirmation>
    {
        private readonly IEmailService _iEmailService;
        private readonly IBulkUploadRepo _iBulkUploadRepo;
        private readonly IApproversRepo _iApproversRepo;
        private readonly ILogger<BulkUploadConfirmEndpoint> _logger;

        public BulkUploadConfirmEndpoint(
            IApproversRepo iApproversRepo,
            IBulkUploadRepo iBulkUploadRepo,
            IEmailService iEmailService,    
            ILogger<BulkUploadConfirmEndpoint> logger)
        {
            _logger = logger;
            _iApproversRepo = iApproversRepo;
            _iBulkUploadRepo = iBulkUploadRepo;
            _iEmailService = iEmailService;
        }

        public override void Configure()
        {
            Post("/bulkuploads/confirm");
            AllowAnonymous();
        }

        public override async Task HandleAsync(BulkUploadConfirmRequest r, CancellationToken ct)
        {
            BulkUploadConfirmResponse response = new BulkUploadConfirmResponse
            {
                Result = false
            };

            try
            {
                BulkUploadConfirmation bulkUploadConfirmation = await MapToEntityAsync(r, ct);

                if (await _iBulkUploadRepo.Confirm(bulkUploadConfirmation, ct))
                {
                    // 1. get list of relevant approvers
                    var approvers = await _iApproversRepo.GetApproversForInvoice(r.InvoiceId, ct);

                    // 2. send out emails
                    response.Result = await _iEmailService.EmailApprovers(approvers, ct);

                    response.Message =  bulkUploadConfirmation.Confirm ? "Bulk upload confirmed" : "Bulk upload deleted";
                }
                else
                {
                    response.Message = "Error confirming bulk upload";
                }

                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                response.Message = ex.Message;

                await SendAsync(response, 400, CancellationToken.None);
            }
        }

        public sealed override async Task<BulkUploadConfirmation> MapToEntityAsync(BulkUploadConfirmRequest r, CancellationToken ct = default)
        {
            var confirmation = await Task.FromResult(new BulkUploadConfirmation());

            confirmation.InvoiceId = r.InvoiceId;
            confirmation.Confirm = r.ConfirmUpload;

            return confirmation;
        }
    }
}