using Tudormobile.OpenTrivia.UI.Services;

namespace OpenTrivia.UI.Tests.Services;

[TestClass]
public class ServiceResultTests
{
    [TestMethod]
    public void Success_CreatesSuccessfulResult()
    {
        // Arrange
        var data = "Test data";

        // Act
        var result = ServiceResult.Success(data);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsFalse(result.IsFailure);
        Assert.AreEqual(data, result.Data);
        Assert.IsNull(result.ErrorMessage);
        Assert.IsNull(result.Exception);
    }

    [TestMethod]
    public void Failure_WithErrorMessage_CreatesFailedResult()
    {
        // Arrange
        var errorMessage = "Something went wrong";

        // Act
        var result = ServiceResult.Failure<string>(errorMessage);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNull(result.Data);
        Assert.AreEqual(errorMessage, result.ErrorMessage);
        Assert.IsNull(result.Exception);
    }

    [TestMethod]
    public void Failure_WithException_CreatesFailedResult()
    {
        // Arrange
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = ServiceResult.Failure<string>(exception);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNull(result.Data);
        Assert.AreEqual("Test exception", result.ErrorMessage);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public void Failure_WithErrorMessageAndException_CreatesFailedResult()
    {
        // Arrange
        var errorMessage = "Custom error message";
        var exception = new InvalidOperationException("Test exception");

        // Act
        var result = ServiceResult.Failure<string>(errorMessage, exception);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNull(result.Data);
        Assert.AreEqual(errorMessage, result.ErrorMessage);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public void Match_WithSuccessfulResult_CallsOnSuccess()
    {
        // Arrange
        var result = ServiceResult.Success("test");
        var onSuccessCalled = false;
        var onFailureCalled = false;

        // Act
        var matchResult = result.Match(
            onSuccess: data =>
            {
                onSuccessCalled = true;
                return data.ToUpper();
            },
            onFailure: error =>
            {
                onFailureCalled = true;
                return "FAILED";
            });

        // Assert
        Assert.IsTrue(onSuccessCalled);
        Assert.IsFalse(onFailureCalled);
        Assert.AreEqual("TEST", matchResult);
    }

    [TestMethod]
    public void Match_WithFailedResult_CallsOnFailure()
    {
        // Arrange
        var result = ServiceResult.Failure<string>("error");
        var onSuccessCalled = false;
        var onFailureCalled = false;

        // Act
        var matchResult = result.Match(
            onSuccess: data =>
            {
                onSuccessCalled = true;
                return "SUCCESS";
            },
            onFailure: error =>
            {
                onFailureCalled = true;
                return error.ToUpper();
            });

        // Assert
        Assert.IsFalse(onSuccessCalled);
        Assert.IsTrue(onFailureCalled);
        Assert.AreEqual("ERROR", matchResult);
    }

    [TestMethod]
    public async Task MatchAsync_WithSuccessfulResult_CallsOnSuccess()
    {
        // Arrange
        var result = ServiceResult.Success("test");
        var onSuccessCalled = false;
        var onFailureCalled = false;

        // Act
        var matchResult = await result.MatchAsync(
            onSuccess: async data =>
            {
                await Task.Delay(1, TestContext.CancellationToken);
                onSuccessCalled = true;
                return data.ToUpper();
            },
            onFailure: async error =>
            {
                await Task.Delay(1, TestContext.CancellationToken);
                onFailureCalled = true;
                return "FAILED";
            });

        // Assert
        Assert.IsTrue(onSuccessCalled);
        Assert.IsFalse(onFailureCalled);
        Assert.AreEqual("TEST", matchResult);
    }

    [TestMethod]
    public async Task MatchAsync_WithFailedResult_CallsOnFailure()
    {
        // Arrange
        var result = ServiceResult.Failure<string>("error");
        var onSuccessCalled = false;
        var onFailureCalled = false;

        // Act
        var matchResult = await result.MatchAsync(
            onSuccess: async data =>
            {
                await Task.Delay(1, TestContext.CancellationToken);
                onSuccessCalled = true;
                return "SUCCESS";
            },
            onFailure: async error =>
            {
                await Task.Delay(1, TestContext.CancellationToken);
                onFailureCalled = true;
                return error.ToUpper();
            });

        // Assert
        Assert.IsFalse(onSuccessCalled);
        Assert.IsTrue(onFailureCalled);
        Assert.AreEqual("ERROR", matchResult);
    }

    [TestMethod]
    public void Match_WithNullData_CallsOnFailure()
    {
        // Arrange
        var result = ServiceResult.Success<string?>(null);
        var onSuccessCalled = false;
        var onFailureCalled = false;

        // Act
        var matchResult = result.Match(
            onSuccess: data =>
            {
                onSuccessCalled = true;
                return "SUCCESS";
            },
            onFailure: error =>
            {
                onFailureCalled = true;
                return "FAILED";
            });

        // Assert
        Assert.IsFalse(onSuccessCalled);
        Assert.IsTrue(onFailureCalled);
        Assert.AreEqual("FAILED", matchResult);
    }

    public TestContext TestContext { get; set; }
}
