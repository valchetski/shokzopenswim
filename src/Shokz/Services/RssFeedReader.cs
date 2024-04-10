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

        logger.LogInformation("\"{FeedName}\" will be downloaded to \"{Location}\"", feed.Title, downloadLocation);
        if (!Directory.Exists(downloadLocation))
        {
            Directory.CreateDirectory(downloadLocation);
        }

        foreach (var item in feed.Items)
        {
            if(item.SpecificItem is MediaRssFeedItem mediaItem)
            {
                var downloadPath = Path.Combine(downloadLocation, $"{mediaItem.Title}.mp3");
                await DownloadFileAsync(mediaItem.Enclosure.Url, downloadPath);
            }
        }
    }

    private async Task DownloadFileAsync(string url, string filePath)
    {
        using var client = new HttpClient();
        using var s = await client.GetStreamAsync(url);
        using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }
}
