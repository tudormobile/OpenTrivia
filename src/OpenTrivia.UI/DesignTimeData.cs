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

    /// <summary>
    /// Gets a sample collection of selectable trivia categories for use in design-time scenarios or testing.
    /// </summary>
    /// <remarks>The collection includes a variety of predefined categories, such as 'General Knowledge' and
    /// several entertainment-related categories. Each category is represented as a selectable item, with some
    /// categories marked as selected by default. This property is intended to provide representative data for UI
    /// development, previews, or demonstration purposes.</remarks>
    public static CategoryCollection SampleCategoryCollection
        => new([
            new SelectableCategory(new TriviaCategory { Id = 9, Name = "General Knowledge" }) { IsSelected = true },
            new SelectableCategory(new TriviaCategory { Id = 10, Name = "Entertainment: Books" }) { IsSelected = false },
            new SelectableCategory(new TriviaCategory { Id = 11, Name = "Entertainment: Film" }) { IsSelected = true },
            new SelectableCategory(new TriviaCategory { Id = 12, Name = "Entertainment: Music" }) { IsSelected = false },
            new SelectableCategory(new TriviaCategory { Id = 13, Name = "Entertainment: Musicals & Theatres" }) { IsSelected = true },
            ]);

    /// <summary>
    /// Gets a static instance of the GameViewModel that is initialized with sample trivia game data.
    /// </summary>
    /// <remarks>Use this property to access a pre-configured GameViewModel for demonstration, design-time
    /// data binding, or testing scenarios. The returned instance is intended for sample or illustrative purposes and
    /// may not reflect actual application state.</remarks>
    public static GameViewModel SampleGameViewModel
        => new(SampleTriviaGame);

    /// <summary>
    /// Gets a sample instance of a trivia game containing a predefined set of questions and categories.
    /// </summary>
    /// <remarks>This property provides a static instance of a trivia game that can be used for demonstration
    /// or testing purposes. The sample game includes a single trivia question in the 'General Knowledge' category,
    /// along with multiple incorrect answers.</remarks>
    public static TriviaGame SampleTriviaGame
        => new(SampleQuestionList, SampleTriviaCategoryList);

    private static List<TriviaQuestion> SampleQuestionList =
        [
            new TriviaQuestion
            {
                Category = new TriviaCategory { Id = 9, Name = "General Knowledge" },
                Type = TriviaQuestionType.MultipleChoice,
                Difficulty = TriviaQuestionDifficulty.Easy,
                Question = "What is the capital of France?",
                CorrectAnswer = "Paris",
                IncorrectAnswers = ["London", "Berlin", "Madrid"]
            }
       ];

    private static List<TriviaCategory> SampleTriviaCategoryList =
        [
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" },
            new TriviaCategory { Id = 12, Name = "Entertainment: Music" },
            new TriviaCategory { Id = 13, Name = "Entertainment: Musicals & Theatres" }
        ];
}
