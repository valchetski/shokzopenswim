namespace Shokz;

public interface IFeedDownloader
{
    Task<string[]> DownloadAsync(Feed feed, string downloadLocation);
}
