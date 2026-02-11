namespace OpenTrivia.Tests;

[TestClass]
public class ApiSessionTokenTests
{
    [TestMethod]
    public void ApiSessionToken_Constructor_ShouldInitializeToken()
    {
        // Arrange
        string expectedToken = "test";

        // Act
        var sessionToken = new ApiSessionToken(expectedToken);

        // Assert
        Assert.AreEqual(expectedToken, sessionToken.Token);
    }

    [TestMethod]
    public void ApiSessionToken_Constructor_ShouldThrowArgumentNullException_WhenTokenIsNull()
    {
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => new ApiSessionToken(null!));
    }

    [TestMethod]
    public void ApiSessionToken_ToString_ShouldReturnToken()
    {
        // Arrange
        string expectedToken = "test";
        var sessionToken = new ApiSessionToken(expectedToken);

        // Act
        string result = sessionToken.ToString();

        // Assert
        Assert.AreEqual(expectedToken, result);
    }

    [TestMethod]
    public void ApiSessionToken_Constructor_ShouldAllowEmptyToken()
    {
        // Arrange
        string expectedToken = "";
        // Act
        var sessionToken = new ApiSessionToken(expectedToken);
        // Assert
        Assert.AreEqual(expectedToken, sessionToken.Token);
    }

    [TestMethod]
    public void ApiSessionToken_InitializeToken_ShouldInitializeToken()
    {
        string expectedToken = "test";
        var sessionToken = new ApiSessionToken { Token = expectedToken };
        Assert.AreEqual(expectedToken, sessionToken.Token);
    }

}
