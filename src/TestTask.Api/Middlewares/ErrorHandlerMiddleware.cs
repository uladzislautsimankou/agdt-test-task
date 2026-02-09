using System.Net;
using System.Text.Json;

namespace TestTask.Api.Middlewares;

internal sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            var errorMessage = string.Empty;

            switch (ex)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorMessage = ex.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Internal server error";
                    _logger.LogError(ex, "Unhandled exception");
                    break;
            }

            var errorResponse = new
            {
                status = context.Response.StatusCode,
                message = errorMessage,
                details = _env.IsDevelopment() ? ex.ToString() : null
            };

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}
