namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents an immutable trivia game containing a collection of questions and their associated categories.
/// </summary>
public class TriviaGame
{
    /// <summary>
    /// Gets the read-only list of trivia questions in this game.
    /// </summary>
    public IReadOnlyList<TriviaQuestion> Questions { get; }

    /// <summary>
    /// Gets the read-only list of trivia categories included in this game.
    /// </summary>
    public IReadOnlyList<TriviaCategory> Categories { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TriviaGame"/> class.
    /// </summary>
    /// <param name="questions">The collection of trivia questions to include in the game.</param>
    /// <param name="categories">The collection of trivia categories to include in the game.</param>
    public TriviaGame(IEnumerable<TriviaQuestion> questions, IEnumerable<TriviaCategory> categories)
    {
        ArgumentNullException.ThrowIfNull(questions);
        ArgumentNullException.ThrowIfNull(categories);

        Questions = questions.ToList().AsReadOnly();
        Categories = categories.ToList().AsReadOnly();
    }
}
