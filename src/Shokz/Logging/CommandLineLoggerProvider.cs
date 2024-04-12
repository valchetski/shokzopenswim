using System.Collections.Concurrent;
using System.CommandLine;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shokz;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("CommandLine")]
public sealed class CommandLineLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private CommandLineLoggerConfiguration _currentConfig;

    private readonly ConcurrentDictionary<string, CommandLineLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly IConsole _console;

    public CommandLineLoggerProvider(
        IConsole console,
        IOptionsMonitor<CommandLineLoggerConfiguration> config)
    {
        _console = console;
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new CommandLineLogger(name, GetCurrentConfig, _console));

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }

    private CommandLineLoggerConfiguration GetCurrentConfig() => _currentConfig;
}
