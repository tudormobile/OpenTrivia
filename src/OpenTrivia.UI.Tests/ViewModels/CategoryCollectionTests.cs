using System.ComponentModel;
using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests.ViewModels;

[TestClass]
public class CategoryCollectionTests
{
    [TestMethod]
    public void CategoryCollection_Constructor_InitializesWithTriviaCategories()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };

        // Act
        var categoryCollection = new CategoryCollection(triviaCategories);

        // Assert
        Assert.IsNotNull(categoryCollection);
        Assert.HasCount(3, categoryCollection);
        Assert.HasCount(0, categoryCollection.SelectedCategories);

        // Verify category content
        Assert.AreEqual(9, categoryCollection[0].Id);
        Assert.AreEqual("General Knowledge", categoryCollection[0].Name);
        Assert.IsFalse(categoryCollection[0].IsSelected);

        Assert.AreEqual(10, categoryCollection[1].Id);
        Assert.AreEqual("Entertainment: Books", categoryCollection[1].Name);
        Assert.IsFalse(categoryCollection[1].IsSelected);

        Assert.AreEqual(11, categoryCollection[2].Id);
        Assert.AreEqual("Entertainment: Film", categoryCollection[2].Name);
        Assert.IsFalse(categoryCollection[2].IsSelected);
    }

    [TestMethod]
    public void CategoryCollection_Constructor_NullTriviaCategories_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new CategoryCollection(null!));
    }

    [TestMethod]
    public void CategoryCollection_SelectCategory_UpdatesSelectedCategories()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };
        var categoryCollection = new CategoryCollection(triviaCategories);

        // Act
        categoryCollection[0].IsSelected = true; // Select the first category

        // Assert
        Assert.HasCount(1, categoryCollection.SelectedCategories);
    }

    [TestMethod]
    public void CategoryCollection_SelectMultipleCategories_UpdatesSelectedCategories()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };
        var categoryCollection = new CategoryCollection(triviaCategories);

        // Act
        categoryCollection[0].IsSelected = true; // Select the first category
        categoryCollection[1].IsSelected = true; // Select the second category
        categoryCollection[2].IsSelected = true; // Select the third category

        // Assert
        Assert.HasCount(3, categoryCollection.SelectedCategories);
    }

    [TestMethod]
    public void CategoryCollection_SelectCategory_RaisesPropertyChanged()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };
        var categoryCollection = new CategoryCollection(triviaCategories);
        var propertyChangedRaised = false;
        categoryCollection.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(CategoryCollection.SelectedCategories))
            {
                propertyChangedRaised = true;
            }
        };
        // Act
        categoryCollection[0].IsSelected = true; // Select the first category
        // Assert
        Assert.IsTrue(propertyChangedRaised, "PropertyChanged event was not raised for SelectedCategories.");
    }

    [TestMethod]
    public void CategoryCollection_SubscribeAndUnsubscribePropertyChanged_SubscribesAndUnsubscribes()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };
        var categoryCollection = new CategoryCollection(triviaCategories);
        var propertyChangedRaisedCount = 0;
        var handler = new PropertyChangedEventHandler((sender, e) =>
         {
             if (e.PropertyName == nameof(CategoryCollection.SelectedCategories))
             {
                 propertyChangedRaisedCount++;
             }
         });
        categoryCollection.PropertyChanged += handler;
        // Act
        categoryCollection[0].IsSelected = true; // Select the first category
        categoryCollection.PropertyChanged -= handler;
        categoryCollection[0].IsSelected = false; // De-Select the first category
        // Assert
        Assert.AreEqual(1, propertyChangedRaisedCount, "Failed to unsubscribe from PropertyChanged.");
    }

    [TestMethod]
    public void CategoryCollection_RemoveCategory_NoLongerRaisesPropertyChanged()
    {
        // Arrange
        var triviaCategories = new[]
        {
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" },
            new TriviaCategory { Id = 11, Name = "Entertainment: Film" }
        };
        var categoryCollection = new CategoryCollection(triviaCategories);
        var propertyChangedRaisedCount = 0;
        var handler = new PropertyChangedEventHandler((sender, e) =>
        {
            if (e.PropertyName == nameof(CategoryCollection.SelectedCategories))
            {
                propertyChangedRaisedCount++;
            }
        });
        var secondCategory = categoryCollection[1];
        categoryCollection.RemoveAt(1);          // Remove the second category
        categoryCollection.PropertyChanged += handler;
        // Act
        secondCategory.IsSelected = true;        // Attempt to select the removed category
        // Assert
        Assert.AreEqual(0, propertyChangedRaisedCount, "Should not have selected a removed category.");
    }
}
