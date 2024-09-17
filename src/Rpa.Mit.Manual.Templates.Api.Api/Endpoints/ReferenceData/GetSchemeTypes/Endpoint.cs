using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace GetSchemeTypes
{
    [ExcludeFromCodeCoverage]
    internal sealed class GetSchemeTypesEndpoint : EndpointWithoutRequest<Response>
    {
        private readonly IReferenceDataRepo _iReferenceDataRepo;
        private readonly ILogger<GetSchemeTypesEndpoint> _logger;

        public GetSchemeTypesEndpoint(
                       ILogger<GetSchemeTypesEndpoint> logger,
                       IReferenceDataRepo iReferenceDataRepo)
        {
            _logger = logger;
            _iReferenceDataRepo = iReferenceDataRepo;
        }

        public override void Configure()
        {
            Get("/schemetypes/get");
            ResponseCache(600); //cache seconds
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = new Response();

            try
            {
                response.SchemeTypes = await _iReferenceDataRepo.GetSchemeTypeReferenceData(ct);

                await SendAsync(response, 200, cancellation: ct);
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