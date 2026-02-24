using Microsoft.AspNetCore.Mvc;
using Tudormobile.OpenTrivia.Extensions;
namespace Tudormobile.OpenTrivia.Service;

/// <summary>
/// Extension methods for configuring the OpenTrivia service endpoints.
/// </summary>
public static class TriviaServiceExtensions
{
    /// <summary>
    /// Adds the OpenTrivia client and service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">Optional action to configure the <see cref="IOpenTriviaClientBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddTriviaService(
        this IServiceCollection services,
        Action<IOpenTriviaClientBuilder>? configure = null)
    {
        services.AddOpenTriviaClient(configure ?? (options => { }));
        services.AddScoped<ITriviaService, TriviaService>();
        services.AddExceptionHandler<TriviaServiceExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }

    /// <summary>
    /// Maps the OpenTrivia service endpoints to the specified web application using the provided URL prefix.
    /// </summary>
    /// <remarks>This method configures HTTP GET and POST routes for trivia-related operations, including
    /// retrieving game status, categories, questions, and creating a new game. Use this extension during application
    /// startup to enable OpenTrivia functionality within the web application.</remarks>
    /// <param name="app">The web application instance to which the OpenTrivia service endpoints are added.</param>
    /// <param name="prefix">The URL prefix used to namespace the OpenTrivia service endpoints. Must be a valid URL segment if it is provided.</param>
    /// <returns>The web application instance with the OpenTrivia service endpoints mapped.</returns>
    public static WebApplication UseTriviaService(this WebApplication app, string prefix = "")
    {
        prefix = prefix.TrimEnd('/');
        app.UseExceptionHandler();

        app.MapGet($"{prefix}/trivia/api/v1", async Task<IResult> (
            HttpContext context, ITriviaService triviaService, CancellationToken cancellationToken)
            => await triviaService.GetStatusAsync(context, cancellationToken));

        app.MapGet($"{prefix}/trivia/api/v1/categories", async Task<IResult> (
            HttpContext context, ITriviaService triviaService, CancellationToken cancellationToken)
            => await triviaService.GetCategoriesAsync(context, cancellationToken));

        app.MapGet($"{prefix}/trivia/api/v1/questions", async Task<IResult> (
            HttpContext context,
            ITriviaService triviaService,
            [FromQuery] int amount = 0,
            [FromQuery] string? category = null,
            [FromQuery] string? difficulty = null,
            [FromQuery] string? type = null,
            [FromQuery] string? encode = null,
            CancellationToken cancellationToken = default)
            => await triviaService.GetQuestionsAsync(context, amount, category, difficulty, type, encode, cancellationToken));

        app.MapGet($"{prefix}/trivia/api/v1/games/{{id}}", async Task<IResult> (
            HttpContext context,
            ITriviaService triviaService,
            [FromRoute] string id,
            CancellationToken cancellationToken = default)
            => await triviaService.GetGameAsync(context, id, cancellationToken));

        app.MapPost($"{prefix}/trivia/api/v1/games", async Task<IResult> (
            HttpContext context, ITriviaService triviaService, CancellationToken cancellationToken)
            => await triviaService.CreateGameAsync(context, cancellationToken));

        app.Logger.LogInformation("{ServiceName} is running", nameof(TriviaService));
        return app;
    }
}
