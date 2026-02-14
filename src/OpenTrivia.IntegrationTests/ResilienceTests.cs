using System.Diagnostics;

namespace OpenTrivia.IntegrationTests;

[TestClass]
[TestCategory("Integration")]
[TestCategory("Performance")]
public class ResilienceTests
{
    private static IOpenTriviaClient _client => TestFixture.SharedClient;
    public TestContext TestContext { get; set; } // MSTest will set this property

    [TestMethod]
    [Timeout(60000, CooperativeCancellation = true)] // 60 second timeout
    public async Task GetQuestions_CompletesWithinTimeout()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        // Act
        var result = await _client.GetQuestionsAsync(10, cancellationToken: TestContext.CancellationToken);
        stopwatch.Stop();

        // Assert
        Assert.IsTrue(result.IsSuccess, $"Failed with response code = '{result.ResponseCode}'");
        Assert.IsLessThan(10000, stopwatch.ElapsedMilliseconds, $"Request took {stopwatch.ElapsedMilliseconds}ms");
    }

    [TestMethod]
    [TestCategory("Stress")]
    public async Task MultipleSequentialRequests_AllSucceed()
    {
        // Arrange
        var results = new List<bool>();

        // Act - Make 5 requests with proper rate limiting
        for (int i = 0; i < 5; i++)
        {
            if (i > 0) await Task.Delay(5000, TestContext.CancellationToken); // Rate limit
            var result = await _client.GetQuestionsAsync(5, cancellationToken: TestContext.CancellationToken);
            results.Add(result.IsSuccess);
        }

        // Assert
        Assert.IsTrue(results.All(r => r), "All requests should succeed");
    }

    [TestMethod]
    [TestCategory("Cancellation")]
    public async Task GetQuestions_RespectsCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var task = _client.GetQuestionsAsync(50, cancellationToken: cts.Token);
        cts.Cancel();

        // Assert
        var result = await task;
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
    }
}