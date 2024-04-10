using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Microsoft.Extensions.Logging;

namespace Shokz;

public class RssFeedReader(ILogger<RssFeedReader> logger) : IFeedReader
{
    public async Task DownloadAsync(string url, string downloadLocation)
    {
        logger.LogInformation("Getting RSS feed info from \"{FeedUrl}\"", url);
        var feed = await FeedReader.ReadAsync(url);
        logger.LogInformation("Got \"{FeedName}\" RSS feed info from \"{FeedUrl}\"", feed.Title, url);

        downloadLocation = Path.Combine(downloadLocation, feed.Title);
        logger.LogInformation("\"{FeedName}\" will be downloaded to \"{Location}\" folder", feed.Title, downloadLocation);
        if (!Directory.Exists(downloadLocation))
        {
            logger.LogInformation("Creating \"{Location}\" folder as it doesn't exist", downloadLocation);
            Directory.CreateDirectory(downloadLocation);
            logger.LogInformation("Created \"{Location}\" folder as it doesn't exist", downloadLocation);
        }

        foreach (var item in feed.Items)
        {
            if(item.SpecificItem is MediaRssFeedItem mediaItem)
            {
                var downloadPath = Path.Combine(downloadLocation, $"{mediaItem.Title}.mp3");
                logger.LogInformation("Downloading \"{File}\" to \"{Location}\"", mediaItem.Title, downloadPath);
                await DownloadFileAsync(mediaItem.Enclosure.Url, downloadPath);
                logger.LogInformation("Downloaded \"{File}\" to \"{Location}\"", mediaItem.Title, downloadPath);
            }
        }
        logger.LogInformation("\"{FeedName}\" downloaded successfully to \"{Location}\" folder", feed.Title, downloadLocation);
    }

    private async Task DownloadFileAsync(string url, string filePath)
    {
        using var client = new HttpClient();
        using var s = await client.GetStreamAsync(url);
        using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }
}
