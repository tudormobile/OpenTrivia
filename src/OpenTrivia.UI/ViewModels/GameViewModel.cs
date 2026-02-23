using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Tudormobile.OpenTrivia.UI.ViewModels;

/// <summary>
/// Represents the view model for managing quiz categories and questions, supporting selection of a single question at a
/// time.
/// </summary>
/// <remarks>This view model exposes collections of selectable categories and questions, and ensures that only one
/// question can be selected at any given time. Property change notifications are raised when the selected question
/// changes, enabling data binding scenarios in UI frameworks.</remarks>
public partial class GameViewModel : ObservableObject
{
    /// <summary>
    /// Gets the collection of categories that can be selected by the user.
    /// </summary>
    /// <remarks>Use this property to access the available categories for selection. The collection is
    /// read-only and each category represents a distinct grouping that can be used for organizing or filtering
    /// items.</remarks>
    public IReadOnlyList<SelectableCategory> Categories { get; }

    /// <summary>
    /// Gets the collection of questions that can be selected by the user.
    /// </summary>
    /// <remarks>The returned collection is read-only and reflects the set of selectable questions available
    /// in the current application context. The contents of the collection may change depending on the application's
    /// state.</remarks>
    public IReadOnlyList<SelectableQuestion> Questions { get; }

    /// <summary>
    /// Gets or sets the currently selected question in the user interface.
    /// </summary>
    /// <remarks>The selected question can be used to display additional details or perform actions related to
    /// that question. This property may be null if no question is selected.</remarks>
    public SelectableQuestion? SelectedQuestion => Questions.FirstOrDefault(q => q.IsSelected);

    /// <summary>
    /// Gets the total number of questions that have been answered correctly.
    /// </summary>
    /// <remarks>Only questions that are marked as answered and have a valid correctness value are included in
    /// the score calculation.</remarks>
    [ObservableProperty]
    public partial int Score { get; private set; }

    /// <summary>
    /// Initializes a new instance of the GameViewModel class using the specified collection of questions.
    /// </summary>
    /// <remarks>The constructor subscribes to the PropertyChanged event of each question in the collection to
    /// monitor property updates. The provided questions are also used to populate the categories and are ordered by
    /// their question number.</remarks>
    /// <param name="questions">The collection of questions to associate with the view model. Cannot be null.</param>
    public GameViewModel(QuestionCollection questions)
    {
        ArgumentNullException.ThrowIfNull(questions);

        Categories = [.. questions
            .DistinctBy(x => x.Category.Id)
            .Select(x => new SelectableCategory(x.Category))
            .OrderBy(x=>x.Name)];

        Questions = [.. questions
            .OrderBy(q => q.QuestionNumber)];

        foreach (var question in Questions)
        {
            PropertyChangedEventManager.AddHandler(question, OnQuestionPropertyChanged, string.Empty);
        }
    }

    /// <summary>
    /// Initializes a new instance of the GameViewModel class using the questions provided by the specified TriviaGame.
    /// </summary>
    /// <param name="game">The TriviaGame instance containing the collection of questions to be used in the GameViewModel. Cannot be null.</param>
    public GameViewModel(TriviaGame game)
        : this(new QuestionCollection(game?.Questions ?? throw new ArgumentNullException(nameof(game)))) { }

    /// <summary>
    /// Resets the game state by clearing all selected answers and resetting the score.
    /// </summary>
    /// <remarks>Call this method to prepare the game for a new round. All questions will be deselected, and
    /// the score will be set to zero. This method is typically used to restart gameplay or begin a new
    /// session.</remarks>
    [RelayCommand]
    private void ResetGame()
    {
        foreach (var question in Questions)
        {
            question.IsSelected = false;
            question.SelectedAnswer = null;
        }
        Score = 0;
    }

    private void OnQuestionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is SelectableQuestion question && e.PropertyName is not null)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectableQuestion.IsSelected):
                    // Deselect all other questions
                    if (question.IsSelected)
                    {
                        foreach (var q in Questions.Where(q => q != question && q.IsSelected))
                        {
                            q.IsSelected = false;
                        }
                    }
                    OnPropertyChanged(nameof(SelectedQuestion));
                    break;
                case nameof(SelectableQuestion.IsAnswered):
                case nameof(SelectableQuestion.IsCorrect):
                    UpdateScore();
                    break;
            }
        }
    }

    private void UpdateScore()
    {
        Score = Questions.Count(question => question.IsAnswered
                                         && question.IsCorrect.HasValue
                                         && question.IsCorrect.Value);
    }

}
