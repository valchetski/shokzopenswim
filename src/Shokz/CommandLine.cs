using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shokz;

public static class CommandLine
{
    public static Parser CreateParser(Action<IServiceCollection>? servicesOverride = null)
    {
        return new CommandLineBuilder(new DownloadCommand())
            .UseHost(_ => new HostBuilder(), (builder) => builder
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    builder.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((ctx, builder) => builder
                    .AddConfiguration(ctx.Configuration.GetSection("Logging"))
                    .AddCommandLineLogger())
                .ConfigureServices((_, services) =>
                {
                    services.AddFeedServices();
                    servicesOverride?.Invoke(services);
                })
                .UseCommandHandler<DownloadCommand, DownloadCommand.Handler>())
                .UseDefaults().Build();    
    }

    public static IServiceCollection AddFeedServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFeedDownloader, FeedDownloader>()
            .AddSingleton<RssFeedReader>()
            .AddSingleton<LocalFeedReader>()
            .AddSingleton<IFeedProcessor, FeedProcessor>();
    }
}
