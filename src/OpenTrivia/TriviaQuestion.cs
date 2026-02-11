namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents a trivia question from the Open Trivia Database.
/// </summary>
public record TriviaQuestion
{
    /// <summary>
    /// The category of the question.
    /// </summary>
    public required TriviaCategory Category { get; init; }

    /// <summary>
    /// The type of the question (multiple choice or true/false).
    /// </summary>
    public required TriviaQuestionType Type { get; init; }

    /// <summary>
    /// The difficulty level of the question (easy, medium, hard).
    /// </summary>
    public required TriviaQuestionDifficulty Difficulty { get; init; }

    /// <summary>
    /// The text of the question.
    /// </summary>
    public required string Question { get; init; }

    /// <summary>
    /// The correct answer to the question.
    /// </summary>
    public required string CorrectAnswer { get; init; }

    /// <summary>
    /// A list of incorrect answers to the question.
    /// </summary>
    public required List<string> IncorrectAnswers { get; init; }
}
