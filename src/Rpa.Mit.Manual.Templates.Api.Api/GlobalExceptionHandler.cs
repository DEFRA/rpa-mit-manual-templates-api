using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.AspNetCore.Diagnostics;

namespace Rpa.Mit.Manual.Templates.Api.Api;

[ExcludeFromCodeCoverage]
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    private const string ExceptionMessage = "An unhandled exception has occurred while executing the request.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception is not null ? exception.Message : ExceptionMessage);

        var problemDetails = CreateProblemDetails(httpContext, exception!);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;

    }

    private static ProblemDetails CreateProblemDetails(in HttpContext httpContext, in Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        switch (exception)
        {
            case NotImplementedException: 
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        return new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method}{ httpContext.Request.Path }"
        };
    }
}