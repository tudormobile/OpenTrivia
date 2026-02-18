using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;
using Tudormobile.OpenTrivia;

namespace WpfSampleApp.ViewModels;

public partial class SelectableQuestion : ObservableObject
{
    private string? _allAnswers;
    private List<String> _answers = [];
    private readonly TriviaQuestion _triviaQuestion;
    public int QuestionNumber { get; init; }

    public string Question => _triviaQuestion.Question;
    public string Answer => _triviaQuestion.CorrectAnswer;
    public List<String> IncorrectAnswers => _triviaQuestion.IncorrectAnswers;
    public string AllAnswers => _allAnswers ??= BuildAnswers();


    public string Difficulty => _triviaQuestion.Difficulty.ToString()!;
    public string Type => _triviaQuestion.Type.ToString()!;
    public string CategoryName => _triviaQuestion.Category.Name;
    public int CategoryId => _triviaQuestion.Category.Id;

    [ObservableProperty]
    public partial bool IsSelected { get; set; } = false;

    [ObservableProperty]
    public partial bool IsAnswered { get; set; } = false;

    public SelectableQuestion(int questionNumber, TriviaQuestion triviaQuestion)
    {
        _triviaQuestion = triviaQuestion;
        QuestionNumber = questionNumber;
    }

    private string BuildAnswers()
    {
        _answers = [.. IncorrectAnswers.Append(Answer).OrderBy(_ => Random.Shared.Next())];
        var sb = new StringBuilder();
        var letter = 'A';

        foreach (var answer in _answers)
        {
            sb.AppendLine($"{letter++}. {answer}");
        }

        return sb.ToString();
    }

}
