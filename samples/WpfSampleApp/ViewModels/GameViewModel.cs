using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Threading;
using WpfSampleApp.Services;

namespace WpfSampleApp.ViewModels;

public partial class GameViewModel : ObservableObject
{
    private int _countdownSeconds;
    private DispatcherTimer _timer;
    private readonly IGameService _gameService;
    private readonly IDialogService _dialogService;
    public List<SelectableCategory> Categories { get; init; }

    [ObservableProperty]
    public partial List<SelectableQuestion>? Questions { get; private set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; } = true;

    [ObservableProperty]
    public partial string Score { get; set; } = "0/0";

    [ObservableProperty]
    public partial SelectableQuestion? SelectedQuestion { get; set; }

    public bool IsCountDownVisible => !string.IsNullOrWhiteSpace(CountDown);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCountDownVisible))]
    public partial string? CountDown { get; set; }

    public GameViewModel(IGameService gameService, IDialogService dialogService, IEnumerable<SelectableCategory> categories)
    {
        _gameService = gameService;
        _dialogService = dialogService;
        Categories = [.. categories];
        Categories.ForEach(category => category.IsSelected = false);

        _timer = new DispatcherTimer(TimeSpan.FromSeconds(1),
            DispatcherPriority.Background,
            CountDownFunction, Dispatcher.CurrentDispatcher);
    }

    /// <summary>
    /// Loads the register entries to display.
    /// </summary>
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadGameAsync()
    {
        IsLoading = true;
        _countdownSeconds = 5 * (Categories.Count - 1);
        CountDown = "- - -";
        _timer.Start();
        var triviaGame = await _gameService.CreateGameAsync(Categories.Select(category => category.ToTriviaCategory()));
        _timer.Stop();
        CountDown = null;

        Questions = [.. triviaGame.Questions.Index().Select(question => new SelectableQuestion(question.Index + 1, question.Item))];

        Score = $"0 / {Questions.Count}";

        IsLoading = false;
    }

    [RelayCommand(CanExecute = nameof(CanAnswer))]
    public void Answer(SelectableQuestion question)
    {
        question.IsAnswered = true;
        Score = $"{Questions?.Count(q => q.IsAnswered)} / {Questions?.Count}";
    }

    public bool CanAnswer(SelectableQuestion question) => !question.IsAnswered;

    partial void OnSelectedQuestionChanged(SelectableQuestion? value)
    {
        var id = value?.CategoryId ?? 0;
        foreach (var category in Categories)
        {
            category.IsSelected = category.Id == id;
        }
    }

    private void CountDownFunction(object? sender, EventArgs e)
    {
        if (_countdownSeconds > 0)
        {
            CountDown = _countdownSeconds.ToString();
            _countdownSeconds--;
        }
        else
        {
            CountDown = "Finalizing Game ...";
        }
    }

}