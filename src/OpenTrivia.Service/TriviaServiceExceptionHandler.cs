using Microsoft.AspNetCore.Diagnostics;

namespace Tudormobile.OpenTrivia.Service;

/// <summary>
/// Global exception handler that catches unhandled exceptions and returns a consistent
/// <c>{ success, error }</c> JSON envelope with an appropriate HTTP status code.
/// </summary>
internal sealed class TriviaServiceExceptionHandler : IExceptionHandler
{
    private readonly ILogger<TriviaServiceExceptionHandler> _logger;

    public TriviaServiceExceptionHandler(ILogger<TriviaServiceExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception in {ServiceName}: {Message}", nameof(TriviaService), exception.Message);

        var (statusCode, message) = exception switch
        {
            HttpRequestException => (StatusCodes.Status502BadGateway, "The upstream trivia API is unavailable."),
            TimeoutException => (StatusCodes.Status504GatewayTimeout, "The upstream trivia API request timed out."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            new { success = false, error = message },
            cancellationToken);

        return true;
    }
}
