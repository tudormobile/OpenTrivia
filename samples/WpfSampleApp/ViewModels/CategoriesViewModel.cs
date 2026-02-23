using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.Services;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace WpfSampleApp.ViewModels;

public partial class CategoriesViewModel : ObservableObject
{
    private readonly IOpenTriviaService _triviaService;
    public CategoryCollection Categories { get; init; }
    public IEnumerable<SelectableCategory> SelectedCategories => Categories.SelectedCategories;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public CategoriesViewModel(IOpenTriviaService triviaService)
    {
        _triviaService = triviaService;
        Categories = new CategoryCollection([]);
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

        if (categories.IsSuccess && categories.Data is IReadOnlyList<TriviaCategory> categoriesList)
        {
            foreach (var category in categoriesList)
            {
                Categories.Add(new SelectableCategory(category));
            }
        }
    }
}
