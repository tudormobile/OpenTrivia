using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Tudormobile.OpenTrivia;

namespace Tudormobile.OpenTrivia.UI.ViewModels;

/// <summary>
/// Represents an observable collection of selectable trivia categories that tracks and exposes the current selection
/// state.
/// </summary>
/// <remarks>This collection is designed to manage a set of trivia categories, allowing clients to observe changes
/// in both the collection itself and the selection state of individual categories. It provides convenient access to the
/// currently selected categories and automatically updates observers when the selection changes. This class is suitable
/// for scenarios where the UI or other components need to react to changes in category selection, such as in trivia or
/// quiz applications.</remarks>
public class CategoryCollection : ObservableCollection<SelectableCategory>
{
    /// <summary>
    /// Gets the collection of categories that are currently selected.
    /// </summary>
    /// <remarks>
    /// Use this property to retrieve only those categories that have been marked as selected by the
    /// user. The returned collection reflects the current selection state and is updated whenever the selection
    /// changes. Note that the implementation will query the underlying collection of categories to determine which 
    /// ones are selected, so it may have performance implications if the collection is large. The Open Trivia
    /// database contains a limited number of categories (they change very infrequently) so this is not a concern.
    /// </remarks>
    public IEnumerable<SelectableCategory> SelectedCategories => Items.Where(c => c.IsSelected);

    /// <summary>
    /// Initializes a new instance of the CategoryCollection class with the specified collection of trivia categories.
    /// </summary>
    /// <param name="categories">The collection of trivia categories to initialize the collection with.</param>
    public CategoryCollection(IEnumerable<TriviaCategory> categories)
    {
        ArgumentNullException.ThrowIfNull(categories);
        foreach (var category in categories)
        {
            var selectableCategory = new SelectableCategory(category);
            Add(selectableCategory); // Add will cause the PropertyChanged event to be subscribed to via the OnCollectionChanged override
        }
    }

    /// <inheritdoc/>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        ForEachCategory(e.OldItems, c => c.PropertyChanged -= OnCategoryPropertyChanged);
        ForEachCategory(e.NewItems, c => c.PropertyChanged += OnCategoryPropertyChanged);
        OnSelectedCategoriesChanged();
    }

    /// <summary>
    /// Invoked when the collection of selected categories changes to notify listeners of the update.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom logic that should occur when the
    /// SelectedCategories property changes. This method raises the PropertyChanged event for the SelectedCategories
    /// property, ensuring that data bindings are updated accordingly.</remarks>
    protected virtual void OnSelectedCategoriesChanged()
        => OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedCategories)));

    private void OnCategoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectableCategory.IsSelected))
        {
            OnSelectedCategoriesChanged();
        }
    }

    private static void ForEachCategory(System.Collections.IList? values, Action<SelectableCategory> action)
    {
        if (values != null)
        {
            foreach (var item in values.OfType<SelectableCategory>())
            {
                action(item);
            }
        }
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    /// <remarks>This event is part of the INotifyPropertyChanged interface and is used to notify clients,
    /// typically binding clients, that a property value has changed. It is included here in order to
    /// allow consumers to subscribe to the event without having to cast the collection to INotifyPropertyChanged.
    /// </remarks>
    public new event PropertyChangedEventHandler? PropertyChanged
    {
        add => ((INotifyPropertyChanged)this).PropertyChanged += value;
        remove => ((INotifyPropertyChanged)this).PropertyChanged -= value;
    }
}
