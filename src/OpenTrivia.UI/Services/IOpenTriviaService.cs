namespace Tudormobile.OpenTrivia.UI.Services;

/// <summary>
/// Provides access to the Open Trivia Database API for retrieving trivia categories and questions.
/// </summary>
public interface IOpenTriviaService
{
    /// <summary>
    /// Retrieves all available trivia categories from the Open Trivia Database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A service result containing a read-only list of trivia categories, or an error if the operation failed.</returns>
    Task<ServiceResult<IReadOnlyList<TriviaCategory>>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specified number of trivia questions from the given categories.
    /// </summary>
    /// <param name="amount">The number of questions to retrieve.</param>
    /// <param name="categories">The categories from which to retrieve questions.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A service result containing a read-only list of trivia questions, or an error if the operation failed.</returns>
    Task<ServiceResult<IReadOnlyList<TriviaQuestion>>> GetQuestionsAsync(int amount, IEnumerable<TriviaCategory> categories, CancellationToken cancellationToken = default);
}
