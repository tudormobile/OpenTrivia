using Tudormobile.OpenTrivia.Extensions;

namespace OpenTrivia.IntegrationTests;

[TestClass]
[TestCategory("Integration")]
public class OpenTriviaIntegrationTests
{
    private static IOpenTriviaClient _client => TestFixture.SharedClient;
    public TestContext TestContext { get; set; } // MSTest will set this property

    [TestMethod]
    [TestCategory("RealApi")]
    public async Task GetCategories_ReturnsValidCategories()
    {
        // Act
        var result = await _client.GetCategoriesAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsNotEmpty(result.Data);
        Assert.IsTrue(result.Data.All(c => c.Id > 0 && !string.IsNullOrEmpty(c.Name)));
    }

    [TestMethod]
    [TestCategory("RealApi")]
    public async Task GetSessionToken_ReturnsValidToken()
    {
        // Act
        var result = await _client.GetSessionTokenAsync(TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data.Token));
    }

    [TestMethod]
    [TestCategory("RealApi")]
    public async Task GetQuestions_WithValidParameters_ReturnsQuestions()
    {
        // Arrange
        var categories = await _client.GetCategoriesAsync(TestContext.CancellationToken);
        var category = categories.Data?.FirstOrDefault();

        // Act
        var result = await _client.GetQuestionsAsync(
            5,
            category,
            TriviaQuestionDifficulty.Easy,
            TriviaQuestionType.MultipleChoice, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.IsTrue(result.Data.Count > 0 && result.Data.Count <= 5);
        Assert.IsTrue(result.Data.All(q =>
            !string.IsNullOrEmpty(q.Question) &&
            !string.IsNullOrEmpty(q.CorrectAnswer)));
    }

    [TestMethod]
    [TestCategory("RealApi")]
    [TestCategory("RateLimit")]
    public async Task RateLimiting_RespectsFiveSecondDelay()
    {
        // Arrange
        var categories = await _client.GetCategoriesAsync(TestContext.CancellationToken);
        var firstTwoCategories = categories.Data?.Take(2).ToList();

        // Act
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = await _client.GetQuestionsAsync(
            5,
            firstTwoCategories!,
            cancellationToken: TestContext.CancellationToken);
        stopwatch.Stop();

        // Assert
        Assert.IsTrue(result.IsSuccess);
        // Should take at least 5 seconds for 2 categories (5 * (2-1))
        Assert.IsGreaterThanOrEqualTo(5000, stopwatch.ElapsedMilliseconds,
            $"Expected at least 5000ms, but was {stopwatch.ElapsedMilliseconds}ms");
    }

    [TestMethod]
    [TestCategory("RealApi")]
    public async Task SessionToken_WorkflowTest()
    {
        // Arrange - Get a token
        var tokenResult = await _client.GetSessionTokenAsync(TestContext.CancellationToken);
        Assert.IsTrue(tokenResult.IsSuccess);
        var token = tokenResult.Data!;

        // Act - Use the token to get questions
        var questionsResult = await _client.GetQuestionsAsync(5, token: token, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(questionsResult.IsSuccess);

        // Act - Reset the token
        var resetResult = await _client.ResetSessionTokenAsync(token, TestContext.CancellationToken);

        // Assert
        Assert.IsTrue(resetResult.IsSuccess);
        Assert.AreEqual(token.Token, resetResult.Data!.Token);
    }
}