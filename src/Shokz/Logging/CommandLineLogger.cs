using System.CommandLine;
using System.CommandLine.IO;
using Microsoft.Extensions.Logging;

namespace Shokz;

public sealed class CommandLineLogger(
    string name,
    Func<CommandLineLoggerConfiguration> getCurrentConfig,
    IConsole console) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        if (getCurrentConfig().LogLevel.TryGetValue(name, out var categoryLevel))
        {
            return categoryLevel <= logLevel;
        }

        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        switch (logLevel)
        {
            case LogLevel.Error:
                console.Error.WriteLine(message);
                break;
            default:
                console.Out.WriteLine(message);
                break;
        }
    }
}