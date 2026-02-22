using Tudormobile.OpenTrivia.UI.ViewModels;

namespace Tudormobile.OpenTrivia.UI;

/// <summary>
/// Provides sample data for design-time previews.
/// </summary>
public static class DesignTimeData
{
    /// <summary>
    /// Gets a sample SelectableQuestion for design-time preview.
    /// </summary>
    public static SelectableQuestion SampleQuestion => new(1, new TriviaQuestion
    {
        Category = new TriviaCategory { Id = 9, Name = "General Knowledge" },
        Type = TriviaQuestionType.MultipleChoice,
        Difficulty = TriviaQuestionDifficulty.Easy,
        Question = "What is the capital of France?",
        CorrectAnswer = "Paris",
        IncorrectAnswers = ["London", "Berlin", "Madrid"]
    });

    /// <summary>
    /// Gets a sample SelectableCategory for design-time preview.
    /// </summary>
    public static SelectableCategory SampleCategory
        => new(new TriviaCategory { Id = 9, Name = "General Knowledge" }) { IsSelected = true };

    /// <summary>
    /// Gets a sample SelectableCategory for design-time preview.
    /// </summary>
    public static SelectableCategory SampleCategoryUnselected
        => new(new TriviaCategory { Id = 9, Name = "General Knowledge" }) { IsSelected = false };

}
