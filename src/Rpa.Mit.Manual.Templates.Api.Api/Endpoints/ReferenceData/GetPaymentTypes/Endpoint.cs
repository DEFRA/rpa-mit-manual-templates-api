using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetPaymentTypes
{
    internal sealed class GetPaymentTypesEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetPaymentTypesEndpoint> _logger;

        public GetPaymentTypesEndpoint(
                       ILogger<GetPaymentTypesEndpoint> logger,
                       IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            AllowAnonymous();
            Get("/paymenttypes/get");
            ResponseCache(600); //cache seconds
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.PaymentTypes = await _iReferenceDataRepo.GetPaymentTypeReferenceData(ct);

                await SendAsync(response, cancellation: ct);
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