using System.Diagnostics.CodeAnalysis;

namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaClientBuilderTests
{
    [TestMethod]
    public void WithLogger_SetsLogger()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();
        var logger = new MockLogger();

        // Act
        builder.AddLogging(logger);

        // Assert
        Assert.AreEqual(logger, builder.Logger);
    }

    [TestMethod]
    public void AddLogger_ReturnsBuilderInstance_ForFluentChaining()
    {
        var builder = new OpenTriviaClientBuilder();

        // Act
        var result = builder.AddLogging(new MockLogger());

        // Assert
        Assert.AreSame(builder, result);

    }

    [TestMethod]
    public void WithHttpClient_ReturnsBuilderInstance_ForFluentChaining()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();
        using var httpClient = new HttpClient();

        // Act
        var result = builder.WithHttpClient(httpClient);

        // Assert
        Assert.AreSame(builder, result);
    }

    [TestMethod]
    public void Build_WithValidConfiguration_ReturnsIOpenTriviaClient()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();
        using var httpClient = new HttpClient();
        builder.WithHttpClient(httpClient);

        // Act
        var client = builder.Build();

        // Assert
        Assert.IsNotNull(client);
        Assert.IsInstanceOfType<IOpenTriviaClient>(client);
    }

    [TestMethod]
    public void Build_WithoutHttpClient_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();

        // Act & Assert
        var exception = Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            builder.Build();
        });

        Assert.Contains("HttpClient instance must be provided", exception.Message);
        Assert.Contains("WithHttpClient()", exception.Message);
    }

    [TestMethod]
    public void WithHttpClient_CanBeCalledMultipleTimes()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();
        using var httpClient1 = new HttpClient();
        using var httpClient2 = new HttpClient();

        // Act
        builder.WithHttpClient(httpClient1);
        builder.WithHttpClient(httpClient2);
        var client = builder.Build();

        // Assert - Should not throw, last HttpClient wins
        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void Build_WithMinimalConfiguration_WorksWithoutLogger()
    {
        // Arrange
        var builder = new OpenTriviaClientBuilder();
        using var httpClient = new HttpClient();

        // Act
        var client = builder
            .WithHttpClient(httpClient)
            .Build();

        // Assert
        Assert.IsNotNull(client);
    }

    [ExcludeFromCodeCoverage]
    class MockLogger : Microsoft.Extensions.Logging.ILogger
    {
#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public IDisposable BeginScope<TState>(TState state) => null!;
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // No-op
        }
    }
}
