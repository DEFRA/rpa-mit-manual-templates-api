using Microsoft.AspNetCore.Diagnostics;

namespace Rpa.Mit.Manual.Templates.Api.Api;
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occured: {Message}", exception.Message);
        //var problemDetails = new ProblemDetails
        //{
        //    Status = StatusCodes.Status500InternalServerError
        //    //Title = "Internal Server Error"
        //};

        //httpContext.Response.StatusCode = problemDetails.Status.Value;
        //await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
