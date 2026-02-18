using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WpfSampleApp.Services;

namespace WpfSampleApp.ViewModels;

public partial class CategoriesViewModel : ObservableObject
{
    private readonly IOpenTriviaService _triviaService;
    public ObservableCollection<SelectableCategory> Categories { get; init; } = [];
    public IEnumerable<SelectableCategory> SelectedCategories => Categories.Where(c => c.IsSelected);
    [ObservableProperty]
    public partial bool IsLoading { get; set; }
    public CategoriesViewModel(IOpenTriviaService triviaService)
    {
        _triviaService = triviaService;
    }

    /// <summary>
    /// Loads the register entries to display.
    /// </summary>
    [RelayCommand(FlowExceptionsToTaskScheduler = true)]
    private async Task LoadCategoriesAsync()
    {
        IsLoading = true;
        var categories = await _triviaService.GetCategoriesAsync();
        IsLoading = false;

        categories.ForEach(category => Categories.Add(new(category)));
    }
}
