using System.Diagnostics.CodeAnalysis;

using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;

[ExcludeFromCodeCoverage]
internal sealed class GetReferenceDataEndpoint : EndpointWithoutRequest<Response>
{
    private readonly IReferenceDataRepo _iReferenceDataRepo;
    private readonly ILogger<GetReferenceDataEndpoint> _logger;

    public GetReferenceDataEndpoint(
                   ILogger<GetReferenceDataEndpoint> logger,
                   IReferenceDataRepo iReferenceDataRepo)
    {
        _logger = logger;
        _iReferenceDataRepo = iReferenceDataRepo;
    }

    public override void Configure()
    {
        Get("/referencedata/getall");
        //ResponseCache(36000); //cache seconds. temp disable till all data is entered and stabilised
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = new Response();

        var me = User.Identity?.Name;

        try
        {
            response.ReferenceData = await _iReferenceDataRepo.GetAllReferenceData(ct);

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