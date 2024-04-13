using Microsoft.Extensions.Logging;

namespace Shokz;

public class FeedDownloader(ILogger<FeedDownloader> logger) : IFeedDownloader
{
    public async Task<string[]> DownloadAsync(Feed feed, string downloadLocation)
    {
        downloadLocation = Path.Combine(downloadLocation, feed.Title);
        logger.LogInformation("\"{FeedName}\" will be downloaded to \"{Location}\" folder", feed.Title, downloadLocation);
        if (!Directory.Exists(downloadLocation))
        {
            logger.LogInformation("Creating \"{Location}\" folder as it doesn't exist", downloadLocation);
            Directory.CreateDirectory(downloadLocation);
            logger.LogInformation("Created \"{Location}\" folder", downloadLocation);
        }

        var downloadedFiles = new List<string>();
        foreach (var item in feed.Items)
        {
            var downloadPath = Path.Combine(downloadLocation, item.Title);
            logger.LogInformation("Downloading \"{File}\" to \"{Location}\"", item.Title, downloadPath);
            switch(UriUtil.GetUriType(item.Uri))
            {
                case UriType.Http:
                    await DownloadFileFromWebAsync(item.Uri, downloadPath);
                    break;
                case UriType.Local:
                    File.Copy(item.Uri, downloadPath, true);
                    break;
            }

            downloadedFiles.Add(downloadPath);
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

        return [.. downloadedFiles];
    }

    private async Task DownloadFileFromWebAsync(string uri, string filePath)
    {
        using var client = new HttpClient();
        using var s = await client.GetStreamAsync(uri);
        using var fs = new FileStream(filePath, FileMode.OpenOrCreate);
        await s.CopyToAsync(fs);
    }
}
