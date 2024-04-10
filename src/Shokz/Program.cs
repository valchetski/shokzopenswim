using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shokz;

[assembly:InternalsVisibleTo("Shokz.Tests")]

var runner = new CommandLineBuilder(new DownloadRssCommand())
    .UseHost(_ => new HostBuilder(), (builder) => builder
        .ConfigureServices((_, services) =>
        {
            services.AddLogging(logger => logger.AddSimpleConsole())
                .AddSingleton<IFeedReader, RssFeedReader>();
        })
        .UseCommandHandler<DownloadRssCommand, DownloadRssCommand.Handler>())
        .UseDefaults().Build();

return await runner.InvokeAsync(args);



