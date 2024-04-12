
using Microsoft.Extensions.Logging;

namespace Shokz;

public abstract class CommonFeedReader(ILogger logger) : IFeedReader
{
    public async Task DownloadAsync(string uri, string downloadLocation)
    {
        var feed = await GetFeedAsync(uri);
        await DownloadAsync(feed, downloadLocation);
    }

    protected async Task DownloadAsync(Feed feed, string downloadLocation)
    {
        downloadLocation = Path.Combine(downloadLocation, feed.Title);
        logger.LogInformation("\"{FeedName}\" will be downloaded to \"{Location}\" folder", feed.Title, downloadLocation);
        if (!Directory.Exists(downloadLocation))
        {
            logger.LogInformation("Creating \"{Location}\" folder as it doesn't exist", downloadLocation);
            Directory.CreateDirectory(downloadLocation);
            logger.LogInformation("Created \"{Location}\" folder", downloadLocation);
        }

        foreach (var item in feed.Items)
        {
            var downloadPath = Path.Combine(downloadLocation, item.Title);
            logger.LogInformation("Downloading \"{File}\" to \"{Location}\"", item.Title, downloadPath);
            await DownloadFileAsync(item.Uri, downloadPath);
            logger.LogInformation("Downloaded \"{File}\" to \"{Location}\"", item.Title, downloadPath);
        }

        if (feed.Items.Count > 0)
        {
            logger.LogInformation("{Count}/{Total} assets from \"{FeedName}\" feed downloaded successfully to \"{Location}\" folder", feed.Items.Count, feed.TotalCount, feed.Title, downloadLocation);
        }
        else
        {
            logger.LogWarning("No assets from \"{FeedName}\" feed were downloaded", feed.Title);
        }
    }

    protected abstract Task<Feed> GetFeedAsync(string uri);

    protected abstract Task DownloadFileAsync(string uri, string filePath);
}
