using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Shokz;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddCommandLineLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, CommandLineLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <CommandLineLoggerConfiguration, CommandLineLoggerProvider>(builder.Services);    

        return builder;
    }
}
