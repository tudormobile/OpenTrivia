namespace OpenTrivia.IntegrationTests;

/// <summary>
/// Provides shared test infrastructure for all integration tests.
/// </summary>
[TestClass]
public class TestFixture
{
    private static HttpClient? _sharedHttpClient;
    private static IOpenTriviaClient? _sharedClient;

    /// <summary>
    /// Gets the shared HttpClient instance for all integration tests.
    /// </summary>
    public static HttpClient SharedHttpClient => _sharedHttpClient
        ?? throw new InvalidOperationException("Test fixture not initialized");

    /// <summary>
    /// Gets the shared OpenTriviaClient instance for all integration tests.
    /// </summary>
    public static IOpenTriviaClient SharedClient => _sharedClient
        ?? throw new InvalidOperationException("Test fixture not initialized");

    /// <summary>
    /// Initializes shared resources once for the entire test assembly.
    /// </summary>
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context)
    {
        // Create a single HttpClient for all integration tests
        _sharedHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30) // Reasonable timeout for API calls
        };

        // Create a shared OpenTriviaClient
        _sharedClient = IOpenTriviaClient.Create(_sharedHttpClient, manageRateLimit: true);

        context.WriteLine("Integration test assembly initialized");
        context.WriteLine($"Using real OpenTrivia API at: {ApiConstants.BaseQuestionUrl}");
    }

    /// <summary>
    /// Cleans up shared resources after all tests complete.
    /// </summary>
    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        _sharedHttpClient?.Dispose();
        _sharedHttpClient = null;
        _sharedClient = null;
    }
    [TestMethod]
    public void CreateClient_ShouldSucceed()
    {
        // Arrange & Act
        var client = IOpenTriviaClient.Create(SharedHttpClient);
        // Assert
        Assert.IsNotNull(client);
    }
}