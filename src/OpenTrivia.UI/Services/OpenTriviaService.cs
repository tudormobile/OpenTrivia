using Microsoft.Extensions.Logging;
using Tudormobile.OpenTrivia.Extensions;

namespace Tudormobile.OpenTrivia.UI.Services;

/// <summary>
/// Implements access to the Open Trivia Database API for retrieving trivia categories and questions.
/// </summary>
internal class OpenTriviaService : IOpenTriviaService
{
    private readonly IOpenTriviaClient _client;
    private readonly ILogger<IOpenTriviaService> _logger;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _categoriesInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenTriviaService"/> class.
    /// </summary>
    /// <param name="client">The Open Trivia API client.</param>
    /// <param name="logger">The logger for diagnostic information.</param>
    public OpenTriviaService(IOpenTriviaClient client, ILogger<IOpenTriviaService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ServiceResult<IReadOnlyList<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await _client.GetCategoriesAsync(cancellationToken);

            if (categories.IsSuccess && categories.Data is not null)
            {
                _categoriesInitialized = true;
                return ServiceResult.Success((IReadOnlyList<TriviaCategory>)categories.Data.AsReadOnly());
            }

            return ServiceResult.Failure<IReadOnlyList<TriviaCategory>>(categories.ErrorMessage ?? "Failed to fetch categories", categories.Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching trivia categories");
            return ServiceResult.Failure<IReadOnlyList<TriviaCategory>>(ex);
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResult<IReadOnlyList<TriviaQuestion>>> GetQuestionsAsync(
        int amount,
        IEnumerable<TriviaCategory> categories,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureCategoriesInitializedAsync(cancellationToken);
            var questions = await _client.GetQuestionsAsync(amount, categories, cancellationToken: cancellationToken);
            return questions.IsSuccess && questions.Data is not null
                ? ServiceResult.Success((IReadOnlyList<TriviaQuestion>)questions.Data.AsReadOnly())
                : ServiceResult.Failure<IReadOnlyList<TriviaQuestion>>(questions.ErrorMessage ?? "Failed to fetch questions", questions.Error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching trivia questions");
            return ServiceResult.Failure<IReadOnlyList<TriviaQuestion>>(ex);
        }
    }

    /// <summary>
    /// Ensures categories are loaded once per service instance in a thread-safe manner.
    /// </summary>
    private async Task EnsureCategoriesInitializedAsync(CancellationToken cancellationToken)
    {
        // Check if categories have already been initialized to avoid unnecessary API calls
        // Follows the double-checked locking pattern to ensure thread safety while minimizing locking overhead
        if (!_categoriesInitialized)
        {
            await _initializationLock.WaitAsync(cancellationToken);
            try
            {
                if (!_categoriesInitialized)
                {
                    // Preload categories to ensure they are available for question retrieval serializer
                    var categories = await _client.GetCategoriesAsync(cancellationToken);
                    _categoriesInitialized = categories.IsSuccess;
                }
            }
            finally
            {
                _initializationLock.Release();
            }
        }
    }
}
