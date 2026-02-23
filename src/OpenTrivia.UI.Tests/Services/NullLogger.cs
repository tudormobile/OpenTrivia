using Microsoft.Extensions.Logging;

namespace OpenTrivia.UI.Tests.Services;

/// <summary>
/// A simple null logger for testing purposes that implements ILogger<T>.
/// </summary>
internal class NullLogger<T> : ILogger<T>
{
    public static NullLogger<T> Instance { get; } = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => false;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Do nothing
    }
}
