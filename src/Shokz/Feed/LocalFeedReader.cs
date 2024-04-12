
using Microsoft.Extensions.Logging;

namespace Shokz;

public class LocalFeedReader(ILogger<LocalFeedReader> logger) : CommonFeedReader(logger)
{
    protected override Task DownloadFileAsync(string uri, string filePath)
    {
        File.Copy(uri, filePath, true);
        return Task.CompletedTask;
    }

    protected override Task<Feed> GetFeedAsync(string uri)
    {
        logger.LogInformation("Getting local feed info from \"{FeedUrl}\"", uri);
        var feedFiles = Directory.GetFiles(uri, "*.mp3");
        logger.LogInformation("Got {TotalCount} files from \"{FeedUrl}\"", feedFiles.Length, uri);

        var feedItems = new List<FeedItem>();
        foreach (var feedFile in feedFiles.OrderBy(x => x))
        {
            feedItems.Add(new FeedItem(Path.GetFileName(feedFile), feedFile));
        }
        return Task.FromResult(new Feed(new DirectoryInfo(uri).Name, feedItems, feedFiles.Length));
    }
}
