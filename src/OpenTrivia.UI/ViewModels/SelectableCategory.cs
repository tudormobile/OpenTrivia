using CommunityToolkit.Mvvm.ComponentModel;
using Tudormobile.OpenTrivia;

namespace Tudormobile.OpenTrivia.UI.ViewModels;

/// <summary>
/// Represents a selectable trivia category that encapsulates a trivia category and tracks its selection state for use
/// in user interfaces.
/// </summary>
/// <remarks>
/// This class is designed to facilitate data binding scenarios where a trivia category needs to be
/// presented with a selectable state, such as in list or toggle button controls. The selection state is managed by the
/// IsSelected property, which can be bound to UI elements to reflect user interaction.
/// </remarks>
public partial class SelectableCategory : ObservableObject
{
    private readonly TriviaCategory _category;

    /// <summary>
    /// Gets the unique identifier for the category.
    /// </summary>
    public int Id => _category.Id;

    /// <summary>
    /// Gets the name of the category.
    /// </summary>
    public string Name => _category.Name;

    /// <summary>
    /// Gets or sets a value indicating whether the item is currently selected.
    /// </summary>
    /// <remarks>
    /// Changing this property typically updates the selection state in the user interface and may
    /// trigger related events or behaviors.
    /// </remarks>
    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    /// <summary>
    /// Creates a new instance of the SelectableCategory class, initializing it with the provided trivia category.
    /// </summary>
    /// <param name="category">The trivia category to encapsulate.</param>
    public SelectableCategory(TriviaCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);
        _category = category;
    }

}
