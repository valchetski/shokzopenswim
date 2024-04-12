namespace Shokz;

public interface IFeedReader
{
    Task<Feed> GetFeedAsync(string uri);
}
