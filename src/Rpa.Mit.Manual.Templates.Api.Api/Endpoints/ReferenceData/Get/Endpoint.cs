using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Rpa.Mit.Manual.Templates.Api.Api.GetReferenceData;

internal sealed class GetReferenceDataEndpoint : EndpointWithoutRequest<Response>
{
    private readonly IReferenceDataRepo _iReferenceDataRepo;

    public GetReferenceDataEndpoint(IReferenceDataRepo iReferenceDataRepo)
    {
        _iReferenceDataRepo = iReferenceDataRepo;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("/referencedata/get");
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        try
        {
            var response = new Response { ReferenceData = await _iReferenceDataRepo.GetAllReferenceData() };

            await SendAsync(response);
        }
        catch (Exception ex)
        {
            // log this

            await SendAsync(new Response());
        }
    }
}