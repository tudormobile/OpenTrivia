using System.Runtime.CompilerServices;

namespace Tudormobile.OpenTrivia.Service
{
    /// <summary>
    /// Provides service methods for handling OpenTrivia API requests and responses.
    /// </summary>
    /// <remarks>
    /// This service acts as a wrapper around the <see cref="IOpenTriviaClient"/> to provide
    /// HTTP endpoint handlers for retrieving trivia categories and service status information.
    /// </remarks>
    public class TriviaService : ITriviaService
    {
        private readonly ILogger _logger;
        private readonly IOpenTriviaClient _openTriviaClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriviaService"/> class.
        /// </summary>
        /// <param name="openTriviaClient">The OpenTrivia client used to communicate with the OpenTrivia API.</param>
        /// <param name="logger">The logger instance for logging diagnostic information.</param>
        public TriviaService(IOpenTriviaClient openTriviaClient, ILogger<TriviaService> logger)
        {
            _openTriviaClient = openTriviaClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the service status including the count of available trivia categories.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IResult"/> containing the service status and available categories information.</returns>
        public async Task<IResult> GetStatusAsync(HttpContext context, CancellationToken cancellationToken)
        {
            LogApiRequest(context);
            var categories = await _openTriviaClient.GetCategoriesAsync(cancellationToken);
            return Results.Ok(new
            {
                success = categories.IsSuccess,
                data = new
                {
                    categories = categories.IsSuccess ? categories.Data : null,
                    message = $"Service running; {categories.Data?.Count ?? 0} categories available."
                }
            });
        }

        /// <summary>
        /// Retrieves all available trivia categories from the OpenTrivia API.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the list of trivia categories if successful,
        /// or a problem result with the error message if the request fails.
        /// </returns>
        public async Task<IResult> GetCategoriesAsync(HttpContext context, CancellationToken cancellationToken)
        {
            LogApiRequest(context);
            var response = await _openTriviaClient.GetCategoriesAsync(cancellationToken);
            if (response.IsSuccess)
            {
                return Results.Ok(new { success = true, data = response.Data });
            }
            return Results.Problem(response.ErrorMessage);
        }

        /// <summary>
        /// Retrieves trivia questions based on the specified parameters.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="amount">The number of questions to retrieve (1-50).</param>
        /// <param name="category">The category ID to filter by, or null for any category.</param>
        /// <param name="difficulty">The difficulty level to filter by (easy, medium, hard), or null for any difficulty.</param>
        /// <param name="type">The question type to filter by (multiple, boolean), or null for any type.</param>
        /// <param name="encode">The encoding type for the response (url3986, base64), or null for default encoding.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the list of trivia questions if successful,
        /// or a problem result with the error message if the request fails.
        /// </returns>
        public async Task<IResult> GetQuestionsAsync(
            HttpContext context,
            int amount,
            string? category = null,
            string? difficulty = null,
            string? type = null,
            string? encode = null,
            CancellationToken cancellationToken = default)
        {
            LogApiRequest(context);

            // Validate amount
            if (amount < 1 || amount > 50)
            {
                return Results.BadRequest(new { success = false, message = "Amount must be between 1 and 50." });
            }

            // Parse optional parameters
            TriviaCategory? triviaCategory = null;
            if (!string.IsNullOrWhiteSpace(category) && int.TryParse(category, out var categoryId))
            {
                triviaCategory = new TriviaCategory { Id = categoryId, Name = string.Empty };
            }

            TriviaQuestionDifficulty? triviaDifficulty = null;
            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                triviaDifficulty = difficulty.ToLowerInvariant() switch
                {
                    "easy" => TriviaQuestionDifficulty.Easy,
                    "medium" => TriviaQuestionDifficulty.Medium,
                    "hard" => TriviaQuestionDifficulty.Hard,
                    _ => null   // Allows all difficulty levels when missing or unrecognized
                };
            }

            TriviaQuestionType? triviaType = null;
            if (!string.IsNullOrWhiteSpace(type))
            {
                triviaType = type.ToLowerInvariant() switch
                {
                    "multiple" => TriviaQuestionType.MultipleChoice,
                    "boolean" => TriviaQuestionType.TrueFalse,
                    _ => null // Allows all question types when missing or unrecognized
                };
            }

            ApiEncodingType? apiEncoding = null;
            if (!string.IsNullOrWhiteSpace(encode))
            {
                apiEncoding = encode.ToLowerInvariant() switch
                {
                    "url3986" => ApiEncodingType.Url3986,
                    "base64" => ApiEncodingType.Base64,
                    _ => ApiEncodingType.Default
                };
            }

            var response = await _openTriviaClient.GetQuestionsAsync(
                amount,
                triviaCategory,
                triviaDifficulty,
                triviaType,
                apiEncoding,
                token: null,
                cancellationToken);

            if (response.IsSuccess)
            {
                return Results.Ok(new { success = true, data = response.Data });
            }
            return Results.Problem(response.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a specific trivia game by its unique identifier.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="id">The unique identifier of the game to retrieve.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the game data if found,
        /// or a problem result if the game is not found or an error occurs.
        /// </returns>
        public async Task<IResult> GetGameAsync(HttpContext context, string id, CancellationToken cancellationToken = default)
        {
            LogApiRequest(context);

            // TODO: Implement game retrieval logic
            await Task.CompletedTask;

            return Results.NotFound(new { success = false, message = $"Game with ID '{id}' not found. Feature not yet implemented." });
        }

        /// <summary>
        /// Creates a new trivia game with the specified configuration.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IResult"/> containing the created game data if successful,
        /// or a problem result with the error message if the creation fails.
        /// </returns>
        public async Task<IResult> CreateGameAsync(HttpContext context, CancellationToken cancellationToken = default)
        {
            LogApiRequest(context);

            // TODO: Implement game creation logic
            // - Read game configuration from request body
            // - Validate configuration
            // - Create game instance
            // - Store game in memory or persistence layer
            // - Return game ID and initial state
            await Task.CompletedTask;

            return Results.StatusCode(StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// Logs information about an API request.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="callerName">The name of the calling method, automatically populated by the compiler.</param>
        private void LogApiRequest(HttpContext context, [CallerMemberName] string callerName = "")
        {
            _logger.LogInformation("{ServiceName}, {CallerName}, {RemoteIpAddress}", nameof(TriviaService), callerName, context.Connection.RemoteIpAddress);
        }
    }
}
