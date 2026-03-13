using System.Net;
using System.Text.Json;

namespace CodeCrafters.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred while processing the request.");
            await HandleExceptionAsync(context);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var payload = JsonSerializer.Serialize(new
        {
            traceId = context.TraceIdentifier,
            message = "An unexpected error occurred. Please try again later."
        });

        return context.Response.WriteAsync(payload);
    }
}

