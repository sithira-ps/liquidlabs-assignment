using liquidlabs_assignment.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Any;

namespace liquidlabs_assignment.Middleware;

public class ExceptionHandlingMiddleware : IExceptionHandler
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ErrorResponse
        {
            status = "failed",
            error = exception.Message,
            details = exception.InnerException?.Message,
        }, ct);

        return true;
    }
}