using CommunityToolkit.Mvvm.ComponentModel;
using Tudormobile.OpenTrivia;

namespace WpfSampleApp.ViewModels;

public partial class SelectableCategory : ObservableObject
{
    private readonly TriviaCategory _category;
    public int Id => _category.Id;
    public string Name => _category.Name;
    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    public SelectableCategory(TriviaCategory category)
    {
        _category = category;
    }

    public TriviaCategory ToTriviaCategory() => new() { Name = Name, Id = Id };
}
