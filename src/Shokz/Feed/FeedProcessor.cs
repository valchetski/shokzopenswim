using Microsoft.Extensions.DependencyInjection;

namespace Shokz;

public class FeedProcessor(
    IFeedDownloader feedDownloader,
    IServiceProvider services) : IFeedProcessor
{
    public async Task DownloadAsync(string uri, string downloadLocation)
    {
        IFeedReader? feedReader = UriUtil.GetUriType(uri) switch
        {
            UriType.Http => services.GetRequiredService<RssFeedReader>(),
            UriType.Local => services.GetRequiredService<LocalFeedReader>(),
            _ => throw new FeedException($"No feed found at \"{uri}\".")
        };
        var feed = await feedReader.GetFeedAsync(uri);
        await feedDownloader.DownloadAsync(feed, downloadLocation);
    }
}
