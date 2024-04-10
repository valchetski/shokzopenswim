namespace Shokz;

public interface IFeedReader
{
    Task DownloadAsync(string url, string downloadLocation);
}
