namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaClientTests
{
    [TestMethod]
    public async Task OpenTriviaClient_GetSessionTokenAsync_ReturnsToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Generated Successfully!"",
                ""token"": ""12345""
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetSessionTokenAsync(cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Token));
        Assert.AreEqual("12345", response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetSessionTokenAsync_WithNullTokenResponse_ReturnsEmptyToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Generated Successfully!"",
                ""token"": null
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetSessionTokenAsync(cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.AreEqual(String.Empty, response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ResetSessionTokenAsync_ReturnsSuccess()
    {
        // Arrange
        var existingToken = new ApiSessionToken("12345");
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""response_message"": ""Token Reset Successfully!"",
                ""token"": ""12345""
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.ResetSessionTokenAsync(existingToken, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(ApiResponseCode.Success, response.ResponseCode);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response.Data.Token));
        Assert.AreEqual(existingToken.Token, response.Data.Token);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ResetSessionTokenAsync_WithMissingToken_ReturnsTokenNotFound()
    {
        // Arrange
        var existingToken = new ApiSessionToken("12345");
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{""response_code"":3,""token"":""12345""}"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.ResetSessionTokenAsync(existingToken, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(ApiResponseCode.TokenNotFound, response.ResponseCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(existingToken.Token, response.Data.Token);
    }

    [DataRow(0, DisplayName = "Zero amount")]
    [DataRow(-1, DisplayName = "Negative amount")]
    [DataRow(51, DisplayName = "Amount exceeds maximum (51)")]
    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithInvalidAmount_Throws(int invalidAmount)
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act & Assert
        await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
        {
            var response = await client.GetQuestionsAsync(invalidAmount, cancellationToken: TestContext.CancellationToken);
        });

    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_NoOptions()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(1, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("amount=1", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithCategory()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var category = new TriviaCategory() { Name = "Test Category", Id = 99 };
        // Act
        var response = await client.GetQuestionsAsync(10, category, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&category=99", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithDifficulty()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, difficulty: TriviaQuestionDifficulty.Hard, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&difficulty=hard", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithType()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, type: TriviaQuestionType.TrueFalse, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&type=boolean", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithTypeMultipleChoice()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetQuestionsAsync(10, type: TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&type=multiple", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithEncoding()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);

        // Act
        var response = await client.GetQuestionsAsync(10, encoding: ApiEncodingType.Base64, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&encode=base64", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetQuestionsAsync_WithToken()
    {
        // Arrange
        using var mockHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": []
            }"
        };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        var token = new ApiSessionToken("123");
        // Act
        var response = await client.GetQuestionsAsync(10, token: token, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.Contains("&token=123", mockHandler.ProvidedRequestUri!.Query);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetCategoriesAsync_ReturnsCategories()
    {
        // Arrange
        var json = @"{
  ""trivia_categories"": [
    {
      ""id"": 9,
      ""name"": ""General Knowledge""
    },
    {
      ""id"": 10,
      ""name"": ""Entertainment: Books""
    },
    {
      ""id"": 11,
      ""name"": ""Entertainment: Film""
    },
    {
      ""id"": 12,
      ""name"": ""Entertainment: Music""
    },
    {
      ""id"": 13,
      ""name"": ""Entertainment: Musicals & Theatres""
    }
  ]
}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetCategoriesAsync(TestContext.CancellationToken);
        // Assert
        Assert.IsTrue(response.IsSuccess);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(5, response.Data);
    }

    [TestMethod]
    public async Task OpenTriviaClient_GetCategoriesAsync_WithNoResults_ReturnsEmptyCategories()
    {
        // Arrange
        var json = @"{}";
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = json };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient);
        // Act
        var response = await client.GetCategoriesAsync(TestContext.CancellationToken);
        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(ApiResponseCode.NoResults, response.ResponseCode);
        Assert.IsNotNull(response.Data);
        Assert.HasCount(0, response.Data);
    }


    public TestContext TestContext { get; set; }
}
