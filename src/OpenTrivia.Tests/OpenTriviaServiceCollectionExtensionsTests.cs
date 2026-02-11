using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tudormobile.OpenTrivia.Extensions;

namespace OpenTrivia.Tests;

[TestClass]
public class OpenTriviaServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddAlphaVantageClient_WithValidConfiguration_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenTriviaClient(builder => { });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<IOpenTriviaClient>();

        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void AddAlphaVantageClient_WithNullServices_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;
        Action<IOpenTriviaClientBuilder> configure = null!;

        // Act & Assert
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            services!.AddOpenTriviaClient(configure!);
        });

        Assert.AreEqual("services", exception.ParamName);
    }

    [TestMethod]
    public void AddAlphaVantageClient_WithNullConfigure_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
        {
            services.AddOpenTriviaClient(null!);
        });

        Assert.AreEqual("configure", exception.ParamName);
    }

    [TestMethod]
    public void AddAlphaVantageClient_RegistersHttpClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenTriviaClient(builder => { });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        Assert.IsNotNull(httpClientFactory);
    }

    [TestMethod]
    public void AddAlphaVantageClient_RegistersClientWithScopedLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenTriviaClient(builder => { });

        // Assert
        var serviceDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(IOpenTriviaClient));

        Assert.IsNotNull(serviceDescriptor);
        Assert.AreEqual(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }

    [TestMethod]
    public void AddAlphaVantageClient_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddOpenTriviaClient(builder => { });

        // Assert
        Assert.AreSame(services, result);
    }

    [TestMethod]
    public void AddAlphaVantageClient_CanBeCalledMultipleTimes()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenTriviaClient(builder => { });
        services.AddOpenTriviaClient(builder => { });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<IOpenTriviaClient>();

        Assert.IsNotNull(client);
    }

    [TestMethod]
    public void AddAlphaVantageClient_ResolvesRequiredDependencies()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenTriviaClient(builder => { });

        // Assert
        var serviceProvider = services.BuildServiceProvider();

        // Verify IHttpClientFactory is available
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Assert.IsNotNull(httpClientFactory);

        // Verify ILogger<OpenTriviaClient> is available
        var logger = serviceProvider.GetRequiredService<ILogger<OpenTriviaClient>>();
        Assert.IsNotNull(logger);

        // Verify IOpenTriviaClient can be resolved
        var client = serviceProvider.GetRequiredService<IOpenTriviaClient>();
        Assert.IsNotNull(client);
    }
}
