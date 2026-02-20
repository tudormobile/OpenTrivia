using CommunityToolkit.Mvvm.ComponentModel;
using Tudormobile.OpenTrivia;

namespace Tudormobile.OpenTrivia.UI.ViewModels;

/// <summary>
/// Represents a trivia question that can be selected by the user, encapsulating the question text, possible answers,
/// and selection state.
/// </summary>
/// <remarks>This class provides properties to manage the selection of answers and evaluate correctness,
/// facilitating interactive trivia experiences. It is designed for use in user interfaces where questions and answers
/// are presented, allowing users to select answers and receive feedback on their choices. The class ensures that answer
/// choices are randomized and supports notification mechanisms for UI updates when selection state changes. Instances
/// must be initialized with a valid trivia question and a positive question number.</remarks>
public partial class SelectableQuestion : ObservableObject
{
    private readonly Lazy<List<string>> _allAnswers;
    private readonly TriviaQuestion _triviaQuestion;

    /// <summary>
    /// Gets the sequential number assigned to the question within the current set.
    /// </summary>
    public int QuestionNumber { get; init; }

    /// <summary>
    /// Gets the question text to be displayed to the user.
    /// </summary>
    public string Question => _triviaQuestion.Question;

    /// <summary>
    /// Gets the correct answer for the associated trivia question.
    /// </summary>
    /// <remarks>This property retrieves the correct answer from the underlying trivia question. It is
    /// read-only and reflects the answer that is considered correct for the current trivia item.</remarks>
    public string CorrectAnswer => _triviaQuestion.CorrectAnswer;

    /// <summary>
    /// Gets the collection of incorrect answer choices for the trivia question.
    /// </summary>
    public IReadOnlyList<string> IncorrectAnswers => _triviaQuestion.IncorrectAnswers;

    /// <summary>
    /// Gets all possible answers (both correct and incorrect) in a randomized order.
    /// </summary>
    /// <remarks>
    /// The answers are randomized once upon first access and cached for the lifetime of the instance to ensure
    /// consistency. The randomization is performed in a thread-safe manner using lazy initialization. Subsequent
    /// accesses return the same randomized list without re-randomizing.
    /// </remarks>
    public IReadOnlyList<string> AllAnswers => _allAnswers.Value;

    /// <summary>
    /// Gets the difficulty level of the trivia question.
    /// </summary>
    public TriviaQuestionDifficulty Difficulty => _triviaQuestion.Difficulty;

    /// <summary>
    /// Gets the type of the trivia question (e.g., multiple choice, true/false).
    /// </summary>
    public TriviaQuestionType Type => _triviaQuestion.Type;

    /// <summary>
    /// Gets the category to which the trivia question belongs.
    /// </summary>
    public TriviaCategory Category => _triviaQuestion.Category;

    /// <summary>
    /// Gets or sets a value indicating whether the item is currently selected.
    /// </summary>
    /// <remarks>This property is typically used in UI scenarios to reflect the selection state of an item in
    /// a list or collection. Changing this property may trigger UI updates or notifications to reflect the selection
    /// change.</remarks>
    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the currently selected answer for the question.
    /// </summary>
    /// <remarks>Changing this property automatically triggers notifications for the IsCorrect and IsAnswered
    /// properties, enabling dynamic updates in the user interface based on the selected answer.</remarks>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCorrect))]
    [NotifyPropertyChangedFor(nameof(IsAnswered))]
    public partial string? SelectedAnswer { get; set; }

    /// <summary>
    /// Gets a value indicating whether the selected answer is correct. Returns <c>null</c> if no answer is selected;
    /// otherwise, returns <see langword="true"/> if the selected answer matches the correct answer, and <see
    /// langword="false"/> if it does not.
    /// </summary>
    /// <remarks>This property evaluates the user's selected answer against the correct answer, providing a
    /// nullable boolean result that reflects the correctness of the selection.</remarks>
    public bool? IsCorrect => SelectedAnswer == null ? null : SelectedAnswer == CorrectAnswer;

    /// <summary>
    /// Gets a value indicating whether an answer has been selected.
    /// </summary>
    /// <remarks>This property returns <see langword="true"/> if the <c>SelectedAnswer</c> is not <see
    /// langword="null"/>, indicating that the user has provided an answer. Otherwise, it returns <see
    /// langword="false"/>.</remarks>
    public bool IsAnswered => SelectedAnswer != null;

    /// <summary>
    /// Initializes a new instance of the SelectableQuestion class using the specified question number and associated
    /// trivia question.
    /// </summary>
    /// <remarks>Throws an ArgumentNullException if triviaQuestion is null, or an ArgumentOutOfRangeException
    /// if questionNumber is less than or equal to zero. The list of possible answers is initialized lazily when first
    /// accessed.</remarks>
    /// <param name="questionNumber">The unique identifier for the question. Must be a positive integer.</param>
    /// <param name="triviaQuestion">The trivia question to associate with this selectable question. Cannot be null.</param>
    public SelectableQuestion(int questionNumber, TriviaQuestion triviaQuestion)
    {
        ArgumentNullException.ThrowIfNull(triviaQuestion);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(questionNumber);
        _triviaQuestion = triviaQuestion;
        QuestionNumber = questionNumber;
        _allAnswers = new Lazy<List<string>>(BuildAnswers);
    }

    /// <summary>
    /// Builds a randomized list of all possible answers by combining correct and incorrect answers.
    /// </summary>
    /// <returns>A list containing all answers in a randomized order.</returns>
    /// <remarks>
    /// This method is invoked once during lazy initialization of the AllAnswers property.
    /// The randomization uses Random.Shared to shuffle answer positions.
    /// </remarks>
    private List<string> BuildAnswers()
        => [.. IncorrectAnswers
            .Append(CorrectAnswer)
            .OrderBy(_ => Random.Shared.Next())];
}
