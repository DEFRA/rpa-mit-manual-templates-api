
using Rpa.Mit.Manual.Templates.Api.Core.Interfaces;

namespace Root;

public class Root : EndpointWithoutRequest<string>
{
    private readonly TimeProvider _timeProvider;

    public Root(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override void Configure()
    {
        AllowAnonymous();
        Get("/");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var message = $"Hello, World @ {_timeProvider.GetUtcNow():hh:mm:ss tt}";

        if (User.Identity?.IsAuthenticated == true)
        {
            message += $"\n{User.Identity.Name}";
        }

        await SendStringAsync(
            message,
            cancellation: ct
        );
    }
}