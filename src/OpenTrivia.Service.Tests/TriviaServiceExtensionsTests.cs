using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace OpenTrivia.Service.Tests;

[TestClass]
public class TriviaServiceExtensionsTests
{
    public TestContext TestContext { get; set; } // MSTest will set this property before each test runs

    #region AddTriviaService Tests

    [TestMethod]
    public void AddTriviaService_RegistersTriviaService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTriviaService();

        // Assert - verify service descriptors were registered
        Assert.Contains(sd => sd.ServiceType == typeof(ITriviaService), services);
        Assert.Contains(sd => sd.ServiceType == typeof(IOpenTriviaClient), services);
    }

    [TestMethod]
    public void AddTriviaService_WithConfigure_RegistersTriviaService()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTriviaService(options => options.WithRateLimitManagement(true));

        // Assert - verify service descriptors were registered
        Assert.Contains(sd => sd.ServiceType == typeof(ITriviaService), services);
        Assert.Contains(sd => sd.ServiceType == typeof(IOpenTriviaClient), services);
    }

    [TestMethod]
    public void AddTriviaService_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddTriviaService();

        // Assert
        Assert.AreSame(services, result);
    }

    #endregion

    #region UseTriviaService Endpoint Mapping Tests

    [TestMethod]
    public async Task UseTriviaService_MapsStatusEndpoint()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_StatusEndpoint_ReturnsSuccessWithData()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1", TestContext.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.CancellationToken);

        // Assert
        using var json = JsonDocument.Parse(content);
        Assert.IsTrue(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.TryGetProperty("data", out _));
    }

    [TestMethod]
    public async Task UseTriviaService_MapsCategoriesEndpoint()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/categories", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_CategoriesEndpoint_ReturnsSuccessWithData()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/categories", TestContext.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.CancellationToken);

        // Assert
        using var json = JsonDocument.Parse(content);
        Assert.IsTrue(json.RootElement.GetProperty("success").GetBoolean());
        Assert.IsTrue(json.RootElement.TryGetProperty("data", out _));
    }

    [TestMethod]
    public async Task UseTriviaService_MapsQuestionsEndpoint_WithValidAmount()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/questions?amount=5", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_QuestionsEndpoint_WithZeroAmount_ReturnsBadRequest()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/questions?amount=0", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_QuestionsEndpoint_AmountExceedsMax_ReturnsBadRequest()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/questions?amount=51", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_QuestionsEndpoint_MissingAmount_ReturnsBadRequest()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/questions", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_MapsGetGameEndpoint_ReturnsNotFound()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/trivia/api/v1/games/test-id", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_MapsCreateGameEndpoint_ReturnsNotImplemented()
    {
        // Arrange
        await using var app = CreateTestApp();
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.PostAsync("/trivia/api/v1/games", null, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotImplemented, response.StatusCode);
    }

    #endregion

    #region UseTriviaService Prefix Tests

    [TestMethod]
    public async Task UseTriviaService_WithPrefix_MapsEndpointsUnderPrefix()
    {
        // Arrange
        await using var app = CreateTestApp(prefix: "/myapi");
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/myapi/trivia/api/v1", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task UseTriviaService_WithTrailingSlashPrefix_NormalizesRoutes()
    {
        // Arrange
        await using var app = CreateTestApp(prefix: "/myapi/");
        await app.StartAsync(TestContext.CancellationToken);
        var client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/myapi/trivia/api/v1", TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region UseTriviaService Return Value

    [TestMethod]
    public void UseTriviaService_ReturnsWebApplication()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddTriviaService();
        builder.Services.AddScoped<IOpenTriviaClient, StubOpenTriviaClient>();
        var app = builder.Build();

        // Act
        var result = app.UseTriviaService();

        // Assert
        Assert.AreSame(app, result);
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Creates a <see cref="WebApplication"/> backed by an in-memory <see cref="TestServer"/>
    /// with a <see cref="StubOpenTriviaClient"/> replacing the real API client.
    /// </summary>
    private static WebApplication CreateTestApp(string prefix = "")
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        // Use real AddTriviaService registration, then override IOpenTriviaClient with stub
        builder.Services.AddTriviaService();
        builder.Services.AddScoped<IOpenTriviaClient, StubOpenTriviaClient>();

        var app = builder.Build();
        app.UseTriviaService(prefix);
        return app;
    }

    #endregion
}
