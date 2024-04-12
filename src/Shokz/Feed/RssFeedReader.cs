
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Microsoft.Extensions.Logging;

namespace Shokz;

public class RssFeedReader(ILogger<RssFeedReader> logger) : IFeedReader
{
    public async Task<Feed> GetFeedAsync(string uri)
    {
        logger.LogInformation("Getting RSS feed info from \"{FeedUrl}\"", uri);
        var rssFeed = await FeedReader.ReadAsync(uri);
        logger.LogInformation("Got \"{FeedName}\" RSS feed info from \"{FeedUrl}\"", rssFeed.Title, uri);

        var feedItems = new List<FeedItem>();
        foreach (var item in rssFeed.Items.OrderBy(x => x.PublishingDate))
        {
            switch(item.SpecificItem)
            {
                case MediaRssFeedItem mediaItem:
                    feedItems.Add(new FeedItem(mediaItem.Title + ".mp3", mediaItem.Enclosure.Url));
                    break;
                case Rss20FeedItem rss20Item:
                    feedItems.Add(new FeedItem(rss20Item.Title + ".mp3", rss20Item.Enclosure.Url));
                    break;
                default:
                    logger.LogWarning("Skipping \"{File}\" download as \"{Type}\" type is not supported", item.Title, item.SpecificItem.GetType());
                    break;
            }
        }
        
        return new Feed(rssFeed.Title, feedItems, rssFeed.Items.Count);
    }
}
