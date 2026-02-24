using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Tudormobile.OpenTrivia.Extensions;

/// <summary>
/// Provides extension methods for registering Open Trivia client services with an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>Use these extension methods to add and configure Open Trivia API clients in ASP.NET Core dependency
/// injection containers. This enables applications to access trivia data from Open Trivia using strongly-typed
/// services.</remarks>
public static class OpenTriviaServiceCollectionExtensions
{
    /// <summary>
    /// Adds OpenTrivia client services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">The action used to configure the <see cref="IOpenTriviaClientBuilder"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddOpenTriviaClient(
        this IServiceCollection services,
        Action<IOpenTriviaClientBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        // Create a builder to capture configuration
        var builder = new OpenTriviaClientBuilder();
        configure(builder);

        // Register HttpClient for OpenTriviaClient
        services.AddHttpClient(nameof(OpenTriviaClient));
        // Register IOpenTriviaClient with a factory that provides the API key
        services.AddSingleton<IOpenTriviaClient>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var logger = sp.GetRequiredService<ILogger<OpenTriviaClient>>();

            // Use the captured options from the builder
            return new OpenTriviaClient(httpClientFactory, logger, builder.Serializer, builder.ManageRateLimit);
        });

        return services;
    }
}