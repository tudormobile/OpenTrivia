using Microsoft.Extensions.Logging;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace Tudormobile.OpenTrivia.UI.Services;

/// <summary>
/// Provides game management services for creating and managing trivia games.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Creates and initializes a new instance of the <see cref="IGameService"/> interface.
    /// </summary>
    /// <param name="openTriviaService">The Open Trivia service used to fetch questions.</param>
    /// <param name="logger">The logger for diagnostic information.</param>
    static IGameService Create(IOpenTriviaService openTriviaService, ILogger<IGameService>? logger = null)
        => new GameService(openTriviaService, logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<IGameService>.Instance);

    /// <summary>
    /// Creates a new trivia game with the specified number of questions from the given categories.
    /// </summary>
    /// <param name="amount">The number of questions to include in the game.</param>
    /// <param name="categories">The categories from which to select questions.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A service result containing the created trivia game, or an error if the operation failed.</returns>
    Task<ServiceResult<TriviaGame>> CreateGameAsync(int amount, IEnumerable<TriviaCategory> categories, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new trivia game with the specified number of questions from the given categories.
    /// </summary>
    /// <param name="amount">The number of questions to include in the game.</param>
    /// <param name="categories">The categories from which to select questions.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A service result containing the created trivia game, or an error if the operation failed.</returns>
    Task<ServiceResult<TriviaGame>> CreateGameAsync(int amount, IEnumerable<SelectableCategory> categories, CancellationToken cancellationToken = default)
        => CreateGameAsync(amount, categories.Select(c => (TriviaCategory)c), cancellationToken);

}
