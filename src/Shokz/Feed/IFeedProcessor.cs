namespace Shokz;

public interface IFeedProcessor
{
    Task DownloadAsync(string uri, string downloadLocation);
}
