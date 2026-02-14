using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia;

internal class OpenTriviaClientBuilder : IOpenTriviaClientBuilder
{
    private HttpClient? _httpClient;
    private ILogger? _logger;
    private IApiDataSerializer? _serializer;
    private bool _manageRateLimit = false;
    private bool _autoDecode = false;
    private ApiEncodingType? _encodingType;

    internal ILogger? Logger => _logger;
    internal IApiDataSerializer? Serializer => _serializer;
    internal bool ManageRateLimit => _manageRateLimit;
    internal bool AutoDecode => _autoDecode;
    internal ApiEncodingType? EncodingType => _encodingType;

    public IOpenTriviaClient Build()
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("An HttpClient instance must be provided. Use WithHttpClient() to indicate what client instance to use.");
        }
        return new OpenTriviaClient(_httpClient, _logger, _serializer, _manageRateLimit, _autoDecode, _encodingType);
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

    public IOpenTriviaClientBuilder UseAutoDecoding(bool autoDecode = true)
    {
        _autoDecode = autoDecode;
        return this;
    }

    public IOpenTriviaClientBuilder WithEncoding(ApiEncodingType encodingType)
    {
        _encodingType = encodingType;
        return this;
    }
}