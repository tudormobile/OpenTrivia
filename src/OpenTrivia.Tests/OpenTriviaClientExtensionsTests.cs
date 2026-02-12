using Tudormobile.OpenTrivia.Extensions;
namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaClientExtensionsTests
{
    private static HttpClient? _httpClient;
    private static HttpMessageHandler? _mockHttpMessageHandler;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _mockHttpMessageHandler = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_mockHttpMessageHandler) { Timeout = TimeSpan.FromSeconds(30) };
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _httpClient!.Dispose();
        _mockHttpMessageHandler?.Dispose();
    }

    [TestMethod]
    public void GetBuilderTest()
    {
        var builder = OpenTriviaClient.GetBuilder();
        Assert.IsInstanceOfType<IOpenTriviaClientBuilder>(builder);
    }

    [TestMethod]
    public void BuildTest()
    {
        var target = OpenTriviaClient.GetBuilder()
            .WithHttpClient(_httpClient!)
            .Build();
        // Assert
        Assert.IsInstanceOfType<IOpenTriviaClient>(target);
    }

    [TestMethod]
    public void Build_WithNullHttpClient_ThrowsArgumentNullException()
    {
        // Arrange
        IOpenTriviaClientBuilder builder = OpenTriviaClient.GetBuilder();
        // Act & Assert
        Assert.ThrowsExactly<ArgumentNullException>(() => builder.WithHttpClient(null!));
    }

    [TestMethod]
    public async Task GetQuestionsAsyncTest()
    {
        // Arrange
        List<TriviaCategory> categories = [
            new TriviaCategory { Id = 9, Name = "General Knowledge" },
            new TriviaCategory { Id = 10, Name = "Entertainment: Books" }
            ];
        using var mockHttpMessageHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": [
                    {
                        ""category"": ""General Knowledge"",
                        ""type"": ""multiple"",
                        ""difficulty"": ""medium"",
                        ""question"": ""What is the capital of France?"",
                        ""correct_answer"": ""Paris"",
                        ""incorrect_answers"": [
                            ""Madrid"",
                            ""Berlin"",
                            ""Rome""
                        ]
                    }
                ]
            }"
        };
        using var httpClient = new HttpClient(mockHttpMessageHandler);
        var client = OpenTriviaClient.GetBuilder().WithHttpClient(httpClient).Build();

        // Act
        var result = await client.GetQuestionsAsync(5, categories, TriviaQuestionDifficulty.Medium, TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsNotEmpty(result.Data);
        Assert.HasCount(2, result.Data);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithNoCategories_ReturnsEmptyList()
    {
        // Arrange
        List<TriviaCategory> categories = [];
        using var mockHttpMessageHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": [
                    {
                        ""category"": ""General Knowledge"",
                        ""type"": ""multiple"",
                        ""difficulty"": ""medium"",
                        ""question"": ""What is the capital of France?"",
                        ""correct_answer"": ""Paris"",
                        ""incorrect_answers"": [
                            ""Madrid"",
                            ""Berlin"",
                            ""Rome""
                        ]
                    }
                ]
            }"
        };
        using var httpClient = new HttpClient(mockHttpMessageHandler);
        var client = OpenTriviaClient.GetBuilder().WithHttpClient(httpClient).Build();

        // Act
        var result = await client.GetQuestionsAsync(5, categories, TriviaQuestionDifficulty.Medium, TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsEmpty(result.Data);
    }

    [TestMethod]
    public async Task GetQuestionsAsync_WithNullCategories_Throws()
    {
        // Arrange
        List<TriviaCategory>? categories = null;
        using var mockHttpMessageHandler = new MockHttpMessageHandler()
        {
            JsonResponse = @"{
                ""response_code"": 0,
                ""results"": [
                    {
                        ""category"": ""General Knowledge"",
                        ""type"": ""multiple"",
                        ""difficulty"": ""medium"",
                        ""question"": ""What is the capital of France?"",
                        ""correct_answer"": ""Paris"",
                        ""incorrect_answers"": [
                            ""Madrid"",
                            ""Berlin"",
                            ""Rome""
                        ]
                    }
                ]
            }"
        };
        using var httpClient = new HttpClient(mockHttpMessageHandler);
        var client = OpenTriviaClient.GetBuilder().WithHttpClient(httpClient).Build();

        // Act & Assert
#pragma warning disable CS8604 // Possible null reference argument.
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => _ = await client.GetQuestionsAsync(5, categories, TriviaQuestionDifficulty.Medium, TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    public TestContext TestContext { get; set; } // MSTest will set this property
}