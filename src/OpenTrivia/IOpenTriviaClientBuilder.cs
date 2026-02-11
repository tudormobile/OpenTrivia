using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia;

/// <summary>
/// Provides a fluent interface for configuring and building instances of <see cref="IOpenTriviaClient"/>.
/// </summary>
public interface IOpenTriviaClientBuilder
{
    /// <summary>
    /// Builds and returns a configured instance of <see cref="IOpenTriviaClient"/> using the settings specified through the builder.
    /// </summary>
    /// <returns>A fully configured <see cref="IOpenTriviaClient"/> instance ready to interact with the Open Trivia Database API.</returns>
    IOpenTriviaClient Build();

    /// <summary>
    /// Configures the builder to use the specified HTTP client for sending requests to the OpenTriviaDB API.
    /// </summary>
    /// <remarks>Use this method to provide a custom-configured HttpClient, for example to set custom headers,
    /// proxies, or timeouts. If not set, a default HttpClient may be used by the implementation.</remarks>
    /// <param name="httpClient">The HTTP client instance to use for all outgoing API requests. Cannot be null. The caller is responsible for
    /// managing the lifetime of the provided client.</param>
    /// <returns>The current builder instance configured to use the specified HTTP client.</returns>
    IOpenTriviaClientBuilder WithHttpClient(HttpClient httpClient);

    /// <summary>
    /// Adds the specified logger to the client builder for capturing diagnostic and operational messages.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging client activity. Cannot be null.</param>
    /// <returns>The current instance of the client builder for method chaining.</returns>
    IOpenTriviaClientBuilder AddLogging(ILogger logger);

}
