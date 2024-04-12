using Microsoft.Extensions.Logging;

namespace Shokz;

public class CommandLineLoggerConfiguration
{
    public int EventId { get; set; }

    public Dictionary<string, LogLevel> LogLevel { get; set; } = [];
}
