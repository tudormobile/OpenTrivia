using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia;

internal class OpenTriviaClientBuilder : IOpenTriviaClientBuilder
{
    private HttpClient? _httpClient;
    private ILogger? _logger;
    private IApiDataSerializer? _serializer;
    private bool _manageRateLimit = false;

    internal ILogger? Logger => _logger;
    internal IApiDataSerializer? Serializer => _serializer;
    internal bool ManageRateLimit => _manageRateLimit;

    public IOpenTriviaClient Build()
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("An HttpClient instance must be provided. Use WithHttpClient() to indicate what client instance to use.");
        }
        return new OpenTriviaClient(_httpClient, _logger, _serializer, _manageRateLimit);
    }

    public IOpenTriviaClientBuilder AddLogging(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public IOpenTriviaClientBuilder WithHttpClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
        return this;
    }

    public IOpenTriviaClientBuilder WithSerializer(IApiDataSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(serializer);
        _serializer = serializer;
        return this;
    }

    public IOpenTriviaClientBuilder WithRateLimitManagement(bool manageRateLimit)
    {
        _manageRateLimit = manageRateLimit;
        return this;
    }
}
