namespace Shokz;

public record Feed(string Title, List<FeedItem> Items, int TotalCount);
