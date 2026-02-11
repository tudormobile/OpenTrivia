namespace OpenTrivia.Tests;

[TestClass]
public class TriviaCategoryTests
{
    [TestMethod]
    public void TriviaCategory_ShouldInitializeProperties()
    {
        // Arrange
        var expectedId = 9;
        var expectedName = "General Knowledge";
        var category = new TriviaCategory()
        {
            Id = expectedId,
            Name = expectedName
        };

        // Act & Assert
        Assert.AreEqual(expectedId, category.Id);
        Assert.AreEqual(expectedName, category.Name);
    }

    [TestMethod]
    public void TriviaCategory_ConstructWith_SetsProperties()
    {
        // Arrange
        var expectedId = 9;
        var expectedName = "General Knowledge";
        var category = new TriviaCategory()
        {
            Id = 123,
            Name = ""
        };

        // Act & Assert
        var newCategory = category with { Id = expectedId, Name = expectedName };
        Assert.AreEqual(expectedId, newCategory.Id);
        Assert.AreEqual(expectedName, newCategory.Name);
    }

    [TestMethod]
    public void TriviaCategory_ToString_ShouldReturnName()
    {
        // Arrange
        var category = new TriviaCategory { Id = 9, Name = "General Knowledge" };

        // Act
        var result = category.ToString();

        // Assert
        Assert.AreEqual("General Knowledge", result);
    }

    [TestMethod]
    public void TriviaCategory_Equals_ShouldReturnTrueForSameId()
    {
        // Arrange
        var category1 = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var category2 = new TriviaCategory { Id = 9, Name = "General Knowledge" };

        // Act
        var result = category1.Equals(category2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TriviaCategory_Equals_ShouldReturnFalseForDifferentId()
    {
        // Arrange
        var category1 = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var category2 = new TriviaCategory { Id = 10, Name = "Books" };

        // Act
        var result = category1.Equals(category2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TriviaCategory_GetHashCode_ShouldBeSameForEqualObjects()
    {
        // Arrange
        var category1 = new TriviaCategory { Id = 9, Name = "General Knowledge" };
        var category2 = new TriviaCategory { Id = 9, Name = "General Knowledge" };

        // Act
        var hash1 = category1.GetHashCode();
        var hash2 = category2.GetHashCode();

        // Assert
        Assert.AreEqual(hash1, hash2);
    }
}
