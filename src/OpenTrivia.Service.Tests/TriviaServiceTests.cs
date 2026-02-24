using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;

namespace OpenTrivia.Service.Tests;

[TestClass]
public class TriviaServiceTests
{
    public TestContext TestContext { get; set; }

    #region GetStatusAsync Tests

    [TestMethod]
    public async Task GetStatusAsync_WithSuccessfulClient_ReturnsOkWithSuccess()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetStatusAsync(context, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(200, GetStatusCode(result));
    }

    [TestMethod]
    public async Task GetStatusAsync_WithSuccessfulClient_ReturnsCategories()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetStatusAsync(context, TestContext.CancellationToken);

        // Assert
        var json = await ExecuteAndParseJsonAsync(result);
        Assert.IsTrue(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.GetProperty("data").TryGetProperty("categories", out _));
    }

    [TestMethod]
    public async Task GetStatusAsync_WithFailedClient_ReturnsSuccessFalse()
    {
        // Arrange
        var (service, client) = CreateService();
        client.CategoriesResponse = new ApiResponse<List<TriviaCategory>>(
            error: new ApiException("API failure"), responseCode: ApiResponseCode.NoResults);
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetStatusAsync(context, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(200, GetStatusCode(result));
        var json = await ExecuteAndParseJsonAsync(result);
        Assert.IsFalse(json.RootElement.GetProperty("success").GetBoolean());
    }

    #endregion

    #region GetCategoriesAsync Tests

    [TestMethod]
    public async Task GetCategoriesAsync_WithSuccessfulClient_ReturnsOk()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetCategoriesAsync(context, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(200, GetStatusCode(result));
        var json = await ExecuteAndParseJsonAsync(result);
        Assert.IsTrue(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.TryGetProperty("data", out _));
    }

    [TestMethod]
    public async Task GetCategoriesAsync_WithFailedClient_ReturnsErrorEnvelope()
    {
        // Arrange
        var (service, client) = CreateService();
        client.CategoriesResponse = new ApiResponse<List<TriviaCategory>>(
            error: new ApiException("Category fetch failed"), responseCode: ApiResponseCode.NoResults);
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetCategoriesAsync(context, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(404, GetStatusCode(result));
        var json = await ExecuteAndParseJsonAsync(result);
        Assert.IsFalse(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.TryGetProperty("error", out _));
    }

    #endregion

    #region GetQuestionsAsync Amount Validation Tests

    [DataRow(0, DisplayName = "Zero amount")]
    [DataRow(-1, DisplayName = "Negative amount")]
    [DataRow(51, DisplayName = "Amount exceeds maximum")]
    [TestMethod]
    public async Task GetQuestionsAsync_WithInvalidAmount_ReturnsBadRequest(int amount)
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetQuestionsAsync(context, amount, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(400, GetStatusCode(result));
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithMinValidAmount_ReturnsOk()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetQuestionsAsync(context, 1, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(200, GetStatusCode(result));
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithMaxValidAmount_ReturnsOk()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetQuestionsAsync(context, 50, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(200, GetStatusCode(result));
    }

    #endregion

    #region GetQuestionsAsync Parameter Parsing Tests

    [TestMethod]
    public async Task GetQuestionsAsync_WithCategory_ParsesCategoryId()
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, category: "9", cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(client.LastQuestionsCategory);
        Assert.AreEqual(9, client.LastQuestionsCategory.Id);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithNonNumericCategory_IgnoresCategory()
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, category: "abc", cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsNull(client.LastQuestionsCategory);
    }

    [DataRow("easy", TriviaQuestionDifficulty.Easy)]
    [DataRow("medium", TriviaQuestionDifficulty.Medium)]
    [DataRow("hard", TriviaQuestionDifficulty.Hard)]
    [DataRow("EASY", TriviaQuestionDifficulty.Easy)]
    [TestMethod]
    public async Task GetQuestionsAsync_WithDifficulty_ParsesDifficulty(string input, TriviaQuestionDifficulty expected)
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, difficulty: input, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(expected, client.LastQuestionsDifficulty);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithUnrecognizedDifficulty_PassesNull()
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, difficulty: "unknown", cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsNull(client.LastQuestionsDifficulty);
    }

    [DataRow("multiple", TriviaQuestionType.MultipleChoice)]
    [DataRow("boolean", TriviaQuestionType.TrueFalse)]
    [DataRow("BOOLEAN", TriviaQuestionType.TrueFalse)]
    [TestMethod]
    public async Task GetQuestionsAsync_WithType_ParsesType(string input, TriviaQuestionType expected)
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, type: input, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(expected, client.LastQuestionsType);
    }

    [DataRow("url3986", ApiEncodingType.Url3986)]
    [DataRow("base64", ApiEncodingType.Base64)]
    [TestMethod]
    public async Task GetQuestionsAsync_WithEncoding_ParsesEncoding(string input, ApiEncodingType expected)
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, encode: input, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(expected, client.LastQuestionsEncoding);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithUnrecognizedEncoding_DefaultsToDefault()
    {
        // Arrange
        var (service, client) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        await service.GetQuestionsAsync(context, 5, encode: "unknown", cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(ApiEncodingType.Default, client.LastQuestionsEncoding);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithFailedClient_ReturnsErrorEnvelope()
    {
        // Arrange
        var (service, client) = CreateService();
        client.QuestionsResponse = new ApiResponse<List<TriviaQuestion>>(
            error: new ApiException("Question fetch failed"), responseCode: ApiResponseCode.NoResults);
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetQuestionsAsync(context, 5, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(404, GetStatusCode(result));
        var json = await ExecuteAndParseJsonAsync(result);
        Assert.IsFalse(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.TryGetProperty("error", out _));
    }

    #endregion

    #region GetGameAsync Tests

    [TestMethod]
    public async Task GetGameAsync_ReturnsNotFound()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.GetGameAsync(context, "test-id", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(404, GetStatusCode(result));
    }

    #endregion

    #region CreateGameAsync Tests

    [TestMethod]
    public async Task CreateGameAsync_ReturnsNotImplemented()
    {
        // Arrange
        var (service, _) = CreateService();
        var context = new DefaultHttpContext();

        // Act
        var result = await service.CreateGameAsync(context, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(501, GetStatusCode(result));
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Creates a <see cref="TriviaService"/> with a <see cref="StubOpenTriviaClient"/> and a null logger.
    /// Returns both so tests can configure the stub before acting.
    /// </summary>
    private static (TriviaService Service, StubOpenTriviaClient Client) CreateService()
    {
        var client = new StubOpenTriviaClient();
        var service = new TriviaService(client, NullLogger<TriviaService>.Instance);
        return (service, client);
    }

    /// <summary>
    /// Extracts the HTTP status code from an <see cref="IResult"/> via the <see cref="IStatusCodeHttpResult"/> interface.
    /// </summary>
    private static int? GetStatusCode(IResult result) => (result as IStatusCodeHttpResult)?.StatusCode;

    /// <summary>
    /// Executes an <see cref="IResult"/> against a <see cref="DefaultHttpContext"/> and parses the response body as JSON.
    /// </summary>
    private static async Task<JsonDocument> ExecuteAndParseJsonAsync(IResult result)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(_ => { });

        var context = new DefaultHttpContext { RequestServices = services.BuildServiceProvider() };
        context.Response.Body = new MemoryStream();
        await result.ExecuteAsync(context);
        context.Response.Body.Position = 0;
        return await JsonDocument.ParseAsync(context.Response.Body);
    }

    #endregion
}
