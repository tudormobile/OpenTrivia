using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tudormobile.OpenTrivia.UI.Services;
using Tudormobile.OpenTrivia.UI.ViewModels;
using WpfSampleApp.Services;
using WpfSampleApp.ViewModels;

namespace WpfSampleApp;

internal partial class MainWindowViewModel : ObservableObject
{
    private readonly IGameService _gameService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    public partial string? Title { get; set; }

    [ObservableProperty]
    public partial CategoriesViewModel? CategoriesModel { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentView))]
    public partial TriviaGameViewModel? GameModel { get; set; }

    public object? CurrentView => GameModel ?? (object?)CategoriesModel;

    public MainWindowViewModel(IGameService gameService, IDialogService dialogService)
    {
        _gameService = gameService;
        _dialogService = dialogService;
    }

    [RelayCommand]
    private void CreateGame(IEnumerable<SelectableCategory> categories)
    {
        if (categories == null || !categories.Any())
        {
            _dialogService.ShowMessage("Please select at least one category.");
            return;
        }
        GameModel = new TriviaGameViewModel(_gameService, _dialogService, categories);
    }

}

