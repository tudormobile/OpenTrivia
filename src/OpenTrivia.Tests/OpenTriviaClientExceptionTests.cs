using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaClientExceptionTests
{
    [TestMethod]
    public async Task OpenTriviaClient_ReturnsError_OnOperationCancelledException()
    {
        // Arrange
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddDebug(); // Outputs to Debug window
            builder.SetMinimumLevel(LogLevel.Debug); // Capture all debug messages
        }).CreateLogger<OpenTriviaClient>();
        using var mockHandler = new MockHttpMessageHandler() { AlwaysThrows = new OperationCanceledException("Simulated cancellation") };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, logger);
        // Act
        var response = await client.GetQuestionsAsync(10, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(ApiResponseCode.Unknown, response.ResponseCode);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ReturnsError_OnJsonException()
    {
        // Arrange
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddDebug(); // Outputs to Debug window
            builder.SetMinimumLevel(LogLevel.Debug); // Capture all debug messages
        }).CreateLogger<OpenTriviaClient>();
        using var mockHandler = new MockHttpMessageHandler() { JsonResponse = "Invalid JSON" };
        using var httpClient = new HttpClient(mockHandler);
        var client = new OpenTriviaClient(httpClient, logger);
        // Act
        var response = await client.GetQuestionsAsync(10, cancellationToken: TestContext.CancellationToken);
        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.IsNull(response.Data);
        Assert.AreEqual(500, response.StatusCode); // JsonException should result in status code 500
        Assert.AreEqual(ApiResponseCode.Unknown, response.ResponseCode);
    }

    [TestMethod]
    public async Task OpenTriviaClient_ReturnsError_OnHttpError()
    {
        // Arrange
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddDebug(); // Outputs to Debug window
            builder.SetMinimumLevel(LogLevel.Debug); // Capture all debug messages
        }).CreateLogger<OpenTriviaClient>();
        using var mockHandler = new MockHttpMessageHandler() { AlwaysThrows = new HttpRequestException("Simulated network error") };
        var client = new OpenTriviaClient(new MockHttpClientFactory(mockHandler), logger);

        // Act
        var response = await client.GetQuestionsAsync(10, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual(500, response.StatusCode); // HttpRequestException should result in status code 500
    }

    [TestMethod]
    public async Task OpenTriviaClient_ReturnsError_OnHttpExceptionWithStatusCode()
    {
        // Arrange
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddDebug(); // Outputs to Debug window
            builder.SetMinimumLevel(LogLevel.Debug); // Capture all debug messages
        }).CreateLogger<OpenTriviaClient>();
        HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable; // Simulate a Service Unavailable error
        using var mockHandler = new MockHttpMessageHandler() { AlwaysThrows = new HttpRequestException("Simulated network error", null, statusCode) };
        var client = new OpenTriviaClient(new MockHttpClientFactory(mockHandler), logger);

        // Act
        var response = await client.GetQuestionsAsync(10, cancellationToken: TestContext.CancellationToken);

        // Assert
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual((int)statusCode, response.StatusCode);
    }

    public TestContext TestContext { get; set; }

    [ExcludeFromCodeCoverage]
    public class MockHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly HttpMessageHandler _handler;
        public MockHttpClientFactory(HttpMessageHandler handler)
        {
            _handler = handler;
        }
        public HttpClient CreateClient(string name)
        {
            return new HttpClient(_handler);
        }

        public void Dispose() => _handler.Dispose();
    }
}
