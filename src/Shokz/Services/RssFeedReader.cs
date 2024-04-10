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

        int downloaded = feed.Items.Count;
        foreach (var item in feed.Items.OrderBy(x => x.PublishingDate))
        {
            switch(item.SpecificItem)
            {
                case MediaRssFeedItem mediaItem:
                    await DownloadFeedFileAsync(mediaItem.Enclosure.Url, downloadLocation, mediaItem.Title);
                    break;
                case Rss20FeedItem rss20Item:
                    await DownloadFeedFileAsync(rss20Item.Enclosure.Url, downloadLocation, rss20Item.Title);
                    break;
                default:
                    downloaded--;
                    logger.LogWarning("Skipping \"{File}\" download as \"{Type}\" type is not supported", item.Title, item.SpecificItem.GetType());
                    break;
            }
        }
        if (downloaded > 0)
        {
            logger.LogInformation("{Count}/{Total} assets from \"{FeedName}\" feed downloaded successfully to \"{Location}\" folder", downloaded, feed.Items.Count, feed.Title, downloadLocation);
        }
        else
        {
            logger.LogWarning("No assets from \"{FeedName}\" feed were downloaded", feed.Title);
        }
    }

    private async Task DownloadFeedFileAsync(string url, string downloadLocation, string fileTitle)
    {
        var downloadPath = Path.Combine(downloadLocation, $"{fileTitle}.mp3");
        logger.LogInformation("Downloading \"{File}\" to \"{Location}\"", fileTitle, downloadPath);
        await DownloadFileAsync(url, downloadPath);
        logger.LogInformation("Downloaded \"{File}\" to \"{Location}\"", fileTitle, downloadPath);
    }

    private async Task DownloadFileAsync(string url, string filePath)
    {
        using var client = new HttpClient();
        using var s = await client.GetStreamAsync(url);
        using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }
}
