using Tudormobile.OpenTrivia;
using Tudormobile.OpenTrivia.UI.ViewModels;

namespace OpenTrivia.UI.Tests.ViewModels;

[TestClass]
public class SelectableCategoryTests
{
    [TestMethod]
    public void SelectableCategory_DefaultConstructorTest()
    {
        // Arrange & Act
        var category = new TriviaCategory()
        {
            Id = 1,
            Name = "General Knowledge"
        };
        var selectableCategory = new SelectableCategory(category);

        // Assert
        Assert.IsNotNull(selectableCategory);
        Assert.IsFalse(selectableCategory.IsSelected);

        // Verify that the properties from TriviaCategory are correctly assigned
        Assert.AreEqual(category.Id, selectableCategory.Id);
        Assert.AreEqual(category.Name, selectableCategory.Name);
    }

    [TestMethod]
    public void SelectableCategory_Constructor_NullTriviaCategory_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new SelectableCategory(null!));
    }

    [TestMethod]
    public void SelectableCategory_IsSelected_CanBeSetToTrue()
    {
        // Arrange
        var category = new TriviaCategory()
        {
            Id = 1,
            Name = "Science"
        };
        var selectableCategory = new SelectableCategory(category);

        // Act
        selectableCategory.IsSelected = true;

        // Assert
        Assert.IsTrue(selectableCategory.IsSelected);
    }

    [TestMethod]
    public void SelectableCategory_IsSelected_CanBeSetToFalse()
    {
        // Arrange
        var category = new TriviaCategory()
        {
            Id = 2,
            Name = "History"
        };
        var selectableCategory = new SelectableCategory(category)
        {
            IsSelected = true
        };

        // Act
        selectableCategory.IsSelected = false;

        // Assert
        Assert.IsFalse(selectableCategory.IsSelected);
    }

    [TestMethod]
    public void SelectableCategory_Id_ReturnsCorrectValue()
    {
        // Arrange
        var expectedId = 42;
        var category = new TriviaCategory()
        {
            Id = expectedId,
            Name = "Geography"
        };
        var selectableCategory = new SelectableCategory(category);

        // Act
        var actualId = selectableCategory.Id;

        // Assert
        Assert.AreEqual(expectedId, actualId);
    }

    [TestMethod]
    public void SelectableCategory_Name_ReturnsCorrectValue()
    {
        // Arrange
        var expectedName = "Entertainment: Film";
        var category = new TriviaCategory()
        {
            Id = 11,
            Name = expectedName
        };
        var selectableCategory = new SelectableCategory(category);

        // Act
        var actualName = selectableCategory.Name;

        // Assert
        Assert.AreEqual(expectedName, actualName);
    }

    [TestMethod]
    public void SelectableCategory_PropertyChanged_RaisedWhenIsSelectedChanges()
    {
        // Arrange
        var category = new TriviaCategory()
        {
            Id = 5,
            Name = "Sports"
        };
        var selectableCategory = new SelectableCategory(category);
        var propertyChangedRaised = false;
        string? changedPropertyName = null;

        selectableCategory.PropertyChanged += (sender, args) =>
        {
            propertyChangedRaised = true;
            changedPropertyName = args.PropertyName;
        };

        // Act
        selectableCategory.IsSelected = true;

        // Assert
        Assert.IsTrue(propertyChangedRaised);
        Assert.AreEqual(nameof(SelectableCategory.IsSelected), changedPropertyName);
    }
}
