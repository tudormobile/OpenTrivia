using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Tudormobile.OpenTrivia;

/// <summary>
/// Implements the <see cref="IOpenTriviaClient"/> interface to provide methods for interacting with the Open Trivia Database API.
/// </summary>
internal class OpenTriviaClient : IOpenTriviaClient
{
    private readonly HttpClient _httpClient;
    private readonly bool _manageRateLimit;
    private readonly ILogger _logger;
    private readonly IApiDataSerializer _serializer;
    private DateTime _lastQuestionRequestTime = DateTime.MinValue;
    private readonly SemaphoreSlim _rateLimitSemaphore = new(1, 1);
    private readonly bool _autoDecode;
    private readonly ApiEncodingType? _encodingType;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenTriviaClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HttpClient class to use.</param>
    /// <param name="logger">Optional logger instance for logging diagnostic information. If null, a NullLogger will be used.</param>
    /// <param name="serializer">Optional serializer instance for serializing and deserializing API data. If null, the internal serializer will be used.</param>
    /// <param name="manageRateLimit">Automatically manage rate limits.</param>
    /// <param name="autoDecode">Automatically decode API responses.</param>
    /// <param name="encodingType">The encoding type to use for API requests.</param>
    internal OpenTriviaClient(HttpClient httpClient,
        ILogger? logger = null,
        IApiDataSerializer? serializer = null,
        bool manageRateLimit = false, bool autoDecode = false, ApiEncodingType? encodingType = null)
    {
        _httpClient = httpClient;
        _manageRateLimit = manageRateLimit;
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
        _serializer = serializer ?? new ApiDataSerializer();
        _autoDecode = autoDecode;
        _encodingType = encodingType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenTriviaClient"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The factory used to create HTTP client instances for sending requests to the API. Cannot be null.</param>
    /// <param name="logger">Optional logger instance for logging diagnostic information. If null, a NullLogger will be used.</param>
    /// <param name="serializer">Optional serializer instance for serializing and deserializing API data. If null, the internal serializer will be used.</param>
    /// <param name="manageRateLimit">Automatically manage rate limits.</param>
    internal OpenTriviaClient(IHttpClientFactory httpClientFactory,
        ILogger? logger = null,
        IApiDataSerializer? serializer = null,
        bool manageRateLimit = false) : this(httpClientFactory.CreateClient(nameof(OpenTriviaClient)), logger, serializer, manageRateLimit) { }

    /// <inheritdoc/>
    public Task<ApiResponse<ApiSessionToken>> GetSessionTokenAsync(CancellationToken cancellationToken = default)
        => GetApiResult(ApiConstants.TokenUrl + "?command=request", _serializer.DeserializeSessionToken, cancellationToken);

    /// <inheritdoc/>
    public Task<ApiResponse<ApiSessionToken>> ResetSessionTokenAsync(ApiSessionToken token, CancellationToken cancellationToken = default)
        => GetApiResult($"{ApiConstants.TokenUrl}?command=reset&token={token}", _serializer.DeserializeSessionToken, cancellationToken);

    /// <inheritdoc/>
    public async Task<ApiResponse<List<TriviaQuestion>>> GetQuestionsAsync(int amount, TriviaCategory? category = null, TriviaQuestionDifficulty? difficulty = null, TriviaQuestionType? type = null, ApiEncodingType? encoding = null, ApiSessionToken? token = null, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount, nameof(amount));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(amount, ApiConstants.MaxAmount, nameof(amount));

        var uriString = $"{ApiConstants.BaseQuestionUrl}?amount={amount}";

        // Category
        if (category is not null)
        {
            uriString += $"&category={category.Id}";
        }

        //Difficulty
        if (difficulty is not null)
        {
            uriString += $"&difficulty={difficulty.ToString()!.ToLower()}";
        }

        // Type
        if (type is not null)
        {
            var t = type == TriviaQuestionType.MultipleChoice ? "multiple" : "boolean";
            uriString += @$"&type={t}";
        }

        // Encoding
        ApiEncodingType decodingType = _encodingType ?? ApiEncodingType.Default;
        if (encoding is not null)
        {
            decodingType = encoding.Value;
            if (encoding.Value != ApiEncodingType.Default)
            {
                uriString += $"&encode={encoding.ToString()!.ToLower()}";
            }
        }
        else if (_encodingType is not null)
        {
            decodingType = _encodingType.Value;
            if (_encodingType.Value != ApiEncodingType.Default)
            {
                uriString += $"&encode={_encodingType.ToString()!.ToLower()}";
            }
        }

        // Token
        if (token is not null)
        {
            uriString += $"&token={token.Token}";
        }

        if (_manageRateLimit)
        {
            try
            {
                await _rateLimitSemaphore.WaitAsync(cancellationToken);
                var timeSinceLastRequest = DateTime.UtcNow - _lastQuestionRequestTime;
                var rateLimitDuration = TimeSpan.FromSeconds(ApiConstants.RateLimitSeconds);

                if (timeSinceLastRequest < rateLimitDuration)
                {
                    var delay = rateLimitDuration - timeSinceLastRequest;
                    _logger.LogDebug("Rate limit active. Waiting {Delay}ms before making request", delay.TotalMilliseconds);
                    await Task.Delay(delay, cancellationToken);
                }

                _lastQuestionRequestTime = DateTime.UtcNow;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Rate limit delay was canceled");
                return new ApiResponse<List<TriviaQuestion>>(error: new ApiException("Rate limit delay was canceled", ex))
                {
                    ResponseCode = ApiResponseCode.Unknown,
                    StatusCode = 499 // Client Closed Request (non-standard, but commonly used to indicate a client-side cancellation)
                };
            }
            finally
            {
                _rateLimitSemaphore.Release();
            }
        }

        // Auto-decode
        Func<JsonDocument, List<TriviaQuestion>> resultBuilder = _autoDecode
            ? doc => _serializer.DeserializeTriviaQuestions(doc, decodingType)
            : doc => _serializer.DeserializeTriviaQuestions(doc);

        return await GetApiResult(uriString, resultBuilder, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<List<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetApiResult(ApiConstants.CategoryUrl, _serializer.DeserializeTriviaCategories, cancellationToken);
        if (result.Data != null)
        {
            result.ResponseCode = result.Data.Count != 0 ? ApiResponseCode.Success : ApiResponseCode.NoResults;
        }
        return result;
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<TriviaQuestionCount>> GetQuestionCountAsync(TriviaCategory category, CancellationToken cancellationToken = default)
    {
        var uriString = $"{ApiConstants.QuestionCountUrl}?category={category.Id}";
        var result = await GetApiResult(uriString, _serializer.DeserializeTriviaQuestionCount, cancellationToken);
        if (result.Data != null)
        {
            result.ResponseCode = ApiResponseCode.Success;
        }
        return result;
    }

    private async Task<ApiResponse<T>> GetApiResult<T>(string uriString, Func<JsonDocument, T> builder, CancellationToken cancellationToken)
    {
        try
        {
            var doc = await GetJsonDocumentAsync(uriString, cancellationToken);
            var data = builder(doc);
            return new ApiResponse<T>(data: data)
            {
                ResponseCode = _serializer.GetResponseCode(doc),
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get API result from URI: {Uri}", uriString);
            return new ApiResponse<T>(error: new ApiException($"Failed to get API result from URI: {uriString}", ex))
            {
                ResponseCode = ApiResponseCode.Unknown,
                StatusCode = ex is HttpRequestException httpEx && httpEx.StatusCode is not null ? (int)httpEx.StatusCode : 500
            };
        }
    }

    private async Task<JsonDocument> GetJsonDocumentAsync(string uriString, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Requesting JSON document from: {Uri}", uriString);

            var stream = await _httpClient.GetStreamAsync(uriString, cancellationToken);
            var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            _logger.LogDebug("Successfully parsed JSON document from: {Uri}", uriString);

            return document;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URI: {Uri}", uriString);
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON response from URI: {Uri}", uriString);
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Request to {Uri} was canceled", uriString);
            throw;
        }
    }
}

