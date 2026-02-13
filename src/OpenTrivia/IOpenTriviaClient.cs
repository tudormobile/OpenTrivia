using Microsoft.Extensions.Logging;

namespace Tudormobile.OpenTrivia;

/// <summary>
/// Defines methods for interacting with the Open Trivia Database API.
/// </summary>
public interface IOpenTriviaClient
{
    /// <summary>
    /// Creates and initializes an instance of the <see cref="IOpenTriviaClient"/> interface.
    /// </summary>
    /// <param name="httpClient">The HttpClient class to use.</param>
    /// <param name="logger">Optional logger instance for logging diagnostic information. If null, a NullLogger will be used.</param>
    /// <param name="serializer">Optional serializer instance for serializing and deserializing API data. If null, the internal serializer will be used.</param>
    /// <param name="manageRateLimit">Automatically manage rate limits.</param>
    public static IOpenTriviaClient Create(HttpClient httpClient,
        ILogger? logger = null,
        IApiDataSerializer? serializer = null,
        bool manageRateLimit = false) => new OpenTriviaClient(httpClient, logger, serializer, manageRateLimit);

    /// <summary>
    /// Retrieves a new session token from the API.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the session token.</returns>
    Task<ApiResponse<ApiSessionToken>> GetSessionTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets an existing session token, clearing its question history.
    /// </summary>
    /// <param name="token">The session token to reset.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the reset session token.</returns>
    Task<ApiResponse<ApiSessionToken>> ResetSessionTokenAsync(ApiSessionToken token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of all available trivia categories from the API.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the list of categories.</returns>
    Task<ApiResponse<List<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves trivia questions from the API with optional filtering.
    /// </summary>
    /// <param name="amount">The number of questions to retrieve (1-50).</param>
    /// <param name="category">The category to filter by, or null for any category.</param>
    /// <param name="difficulty">The difficulty level to filter by, or null for any difficulty.</param>
    /// <param name="type">The question type to filter by, or null for any type.</param>
    /// <param name="encoding">The encoding type for the response, or null for default encoding.</param>
    /// <param name="token">The session token to use, or null for no session.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the list of trivia questions.</returns>
    Task<ApiResponse<List<TriviaQuestion>>> GetQuestionsAsync(
        int amount,
        TriviaCategory? category = null,
        TriviaQuestionDifficulty? difficulty = null,
        TriviaQuestionType? type = null,
        ApiEncodingType? encoding = null,
        ApiSessionToken? token = null,
        CancellationToken cancellationToken = default);

}