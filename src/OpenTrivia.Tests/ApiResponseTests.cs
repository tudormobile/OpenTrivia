namespace OpenTrivia.Tests;

[TestClass]
public class ApiResponseTests
{
    [TestMethod]
    public void ApiResponse_SuccessfulResponse_ShouldHaveDataAndNoError()
    {
        // Arrange
        var expectedData = new { Question = "What is the capital of France?", Answer = "Paris" };
        var apiResponse = new ApiResponse<object>(expectedData, responseCode: ApiResponseCode.Success, statusCode: 200);

        // Act
        var actualData = apiResponse.Data;
        var actualError = apiResponse.Error;
        var actualIsSuccess = apiResponse.IsSuccess;

        // Assert
        Assert.IsNotNull(actualData);
        Assert.AreEqual(expectedData, actualData);
        Assert.IsNull(actualError);
        Assert.IsNull(apiResponse.ErrorMessage);
        Assert.AreEqual(ApiResponseCode.Success, apiResponse.ResponseCode);
        Assert.AreEqual(200, apiResponse.StatusCode);
        Assert.IsTrue(actualIsSuccess);
    }

    [TestMethod]
    public void ApiResponse_ErrorResponse_ShouldHaveErrorAndNoData()
    {
        // Arrange
        var expectedError = new ApiException("An error occurred.");
        var apiResponse = new ApiResponse<object>(error: expectedError, responseCode: ApiResponseCode.InvalidParameter, statusCode: 400);

        // Act
        var actualData = apiResponse.Data;
        var actualError = apiResponse.Error;
        var actualIsSuccess = apiResponse.IsSuccess;

        // Assert
        Assert.IsNull(actualData);
        Assert.IsNotNull(actualError);
        Assert.AreEqual(expectedError.Message, actualError.Message);
        Assert.AreEqual(ApiResponseCode.InvalidParameter, apiResponse.ResponseCode);
        Assert.AreEqual(400, apiResponse.StatusCode);
        Assert.AreEqual("An error occurred.", apiResponse.ErrorMessage);
        Assert.IsFalse(actualIsSuccess);
    }
}
