

using System.Diagnostics.CodeAnalysis;

namespace Root;

[ExcludeFromCodeCoverage]
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

        await SendStringAsync(
            message,
            cancellation: ct
        );
    }
}