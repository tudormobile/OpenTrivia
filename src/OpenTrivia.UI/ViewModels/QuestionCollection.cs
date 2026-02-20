using System.Collections.ObjectModel;

namespace Tudormobile.OpenTrivia.UI.ViewModels;

/// <summary>
/// Represents a collection of selectable trivia questions, each assigned a unique sequential number for identification
/// and ordering.
/// </summary>
/// <remarks>The collection is initialized with a set of trivia questions, each wrapped in a SelectableQuestion
/// object. The collection automatically assigns a sequential number to each question as it is added. This class is
/// intended to simplify the management and presentation of trivia questions in user interfaces or quiz
/// applications.</remarks>
public class QuestionCollection : ObservableCollection<SelectableQuestion>
{
    /// <summary>
    /// Initializes a new instance of the QuestionCollection class using the specified collection of trivia questions.
    /// </summary>
    /// <remarks>Each trivia question in the collection is wrapped in a SelectableQuestion and assigned a
    /// unique question number for identification.</remarks>
    /// <param name="triviaQuestions">An enumerable collection of TriviaQuestion objects to be added to the collection. This parameter cannot be null.</param>
    public QuestionCollection(IEnumerable<TriviaQuestion> triviaQuestions)
    {
        ArgumentNullException.ThrowIfNull(triviaQuestions);
        int questionNumber = 1;
        foreach (var triviaQuestion in triviaQuestions)
        {
            Add(new SelectableQuestion(questionNumber++, triviaQuestion));
        }
    }

}
