using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Threading;
using Tudormobile.OpenTrivia.UI.Services;
using Tudormobile.OpenTrivia.UI.ViewModels;
using WpfSampleApp.Services;

namespace WpfSampleApp.ViewModels;

public partial class TriviaGameViewModel : ObservableObject
{
    // use GameViewModel from the UI Library
    private int _countdownSeconds;
    private readonly DispatcherTimer _timer;
    private readonly IGameService _gameService;
    private readonly IDialogService _dialogService;
    private List<SelectableCategory> _categories;

    [ObservableProperty]
    public partial GameViewModel? Game { get; private set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; } = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AnswerCommand))]
    public partial SelectableQuestion? SelectedQuestion { get; set; }

    public bool IsCountDownVisible => !string.IsNullOrWhiteSpace(CountDown);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCountDownVisible))]
    public partial string? CountDown { get; set; }

    public TriviaGameViewModel(IGameService gameService, IDialogService dialogService, IEnumerable<SelectableCategory> categories)
    {
        _gameService = gameService;
        _dialogService = dialogService;
        _categories = [.. categories];
        _categories.ForEach(c => c.IsSelected = false);

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
        _countdownSeconds = 5 * (_categories.Count - 1);
        CountDown = "- - -";
        _timer.Start();
        var triviaGame = await _gameService.CreateGameAsync(50, _categories);
        _timer.Stop();
        CountDown = null;

        if (triviaGame.IsSuccess)
        {
            Game = new GameViewModel(triviaGame.Data!);
        }
        else
        {
            _dialogService.ShowMessage($"Error: Failed to load game: {triviaGame.ErrorMessage}");
        }

        IsLoading = false;
    }

    [RelayCommand(CanExecute = nameof(CanAnswer))]
    public void Answer(SelectableQuestion question)
    {
        question.SelectedAnswer = question.CorrectAnswer;
        AnswerCommand.NotifyCanExecuteChanged();
    }

    public static bool CanAnswer(SelectableQuestion question) => question is not null && !question.IsAnswered;

    partial void OnSelectedQuestionChanged(SelectableQuestion? value)
    {
        var id = value?.Category.Id ?? 0;
        if (Game is not null)
        {
            foreach (var category in Game.Categories)
            {
                category.IsSelected = category.Id == id;
            }
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