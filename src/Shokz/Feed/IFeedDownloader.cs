namespace Shokz;

public interface IFeedDownloader
{
    Task DownloadAsync(Feed feed, string downloadLocation);
}
