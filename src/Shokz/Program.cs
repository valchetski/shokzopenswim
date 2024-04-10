using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shokz;

[assembly:InternalsVisibleTo("Shokz.Tests")]

var serviceProvider = new ServiceCollection()
    .AddLogging(logger => logger.AddSimpleConsole())
    .AddSingleton<IFeedReader, RssFeedReader>()
    .BuildServiceProvider();

var feedReader = serviceProvider.GetRequiredService<IFeedReader>();
await feedReader.DownloadAsync(args[0], args[1]);

