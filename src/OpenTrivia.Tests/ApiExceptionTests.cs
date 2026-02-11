namespace OpenTrivia.Tests;

[TestClass]
public class ApiExceptionTests
{
    [TestMethod]
    public void ApiException_DefaultConstructorTest()
    {
        // Arrange & Act
        var exception = new ApiException();

        // Assert
        Assert.IsInstanceOfType<ApiException>(exception);
    }

    [TestMethod]
    public void ApiException_ConstructorWithMessage_SetsMessage()
    {
        // Arrange
        var message = "this is an exception message";

        // Act
        var exception = new ApiException(message);

        // Assert
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void ApiException_ConstructorWithInnerException_SetsInnerException()
    {
        // Arrange
        var inner = new Exception();
        var message = "this is an exception message";

        // Act
        var exception = new ApiException(message, inner);

        // Assert
        Assert.AreEqual(message, exception.Message);
        Assert.AreEqual(inner, exception.InnerException);
    }

}
