namespace Shokz;

public interface IFeedReader
{
    Task DownloadAsync(string uri, string downloadLocation);
}
