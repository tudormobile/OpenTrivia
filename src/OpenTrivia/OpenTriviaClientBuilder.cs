using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia;

internal class OpenTriviaClientBuilder : IOpenTriviaClientBuilder
{
    private HttpClient? _httpClient;
    private ILogger? _logger;

    public ILogger? Logger => _logger;

    public IOpenTriviaClient Build()
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("An HttpClient instance must be provided. Use WithHttpClient() to indicate what client instance to use.");
        }
        return new OpenTriviaClient(_httpClient, _logger);
    }

    public IOpenTriviaClientBuilder AddLogging(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public IOpenTriviaClientBuilder WithHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        return this;
    }
}
