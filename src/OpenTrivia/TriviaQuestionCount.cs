namespace Tudormobile.OpenTrivia;

/// <summary>
/// Represents the number of trivia questions available in a specific category, broken down by difficulty level.
/// </summary>
/// <remarks>Use this record to access the total count of questions as well as the counts for each difficulty
/// level—easy, medium, and hard—within a given trivia category. This information can be used to determine question
/// availability for quiz generation or to display statistics to users.</remarks>
public record TriviaQuestionCount
{
    /// <summary>
    /// The total number of questions available for the specified category.
    /// </summary>
    public int TotalQuestionCount { get; set; }
    /// <summary>
    /// The number of questions available for the specified category that are of "easy" difficulty.
    /// </summary>
    public int EasyQuestionCount { get; set; }
    /// <summary>
    /// The number of questions available for the specified category that are of "medium" difficulty.
    /// </summary>
    public int MediumQuestionCount { get; set; }
    /// <summary>
    /// The number of questions available for the specified category that are of "hard" difficulty.
    /// </summary>
    public int HardQuestionCount { get; set; }
}
