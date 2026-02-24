namespace Tudormobile.OpenTrivia.Service
{
    /// <summary>
    /// Defines the contract for handling OpenTrivia API requests and responses as HTTP endpoint handlers.
    /// </summary>
    public interface ITriviaService
    {
        /// <summary>
        /// Creates a new trivia game with the specified configuration.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IResult"/> containing the created game data if successful,
        /// or a problem result with the error message if the creation fails.</returns>
        Task<IResult> CreateGameAsync(HttpContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all available trivia categories from the OpenTrivia API.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IResult"/> containing the list of trivia categories if successful,
        /// or a problem result with the error message if the request fails.</returns>
        Task<IResult> GetCategoriesAsync(HttpContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a specific trivia game by its unique identifier.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="id">The unique identifier of the game to retrieve.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IResult"/> containing the game data if found,
        /// or a problem result if the game is not found or an error occurs.</returns>
        Task<IResult> GetGameAsync(HttpContext context, string id, CancellationToken cancellationToken = default);

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
        /// <returns>An <see cref="IResult"/> containing the list of trivia questions if successful,
        /// or a problem result with the error message if the request fails.</returns>
        Task<IResult> GetQuestionsAsync(HttpContext context, int amount, string? category = null, string? difficulty = null, string? type = null, string? encode = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the service status including the count of available trivia categories.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IResult"/> containing the service status and available categories information.</returns>
        Task<IResult> GetStatusAsync(HttpContext context, CancellationToken cancellationToken);
    }
}