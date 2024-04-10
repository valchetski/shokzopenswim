using System.CommandLine;
using System.CommandLine.Invocation;

namespace Shokz;

public class DownloadRssCommand : RootCommand
{
    public DownloadRssCommand()
        : base("Download items from RSS feed")
    {
        AddArgument(new Argument<string>("url", "Url to the RSS feed."));
        AddOption(new Option<string>("--output", "Path to download RSS feed items.")
        {
            IsRequired = true,
        });
    }

    public new class Handler(IFeedReader feedReader) : ICommandHandler
    {
        public string? Url { get; set; }
        public string? Output { get; set; }

        public int Invoke(InvocationContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await feedReader.DownloadAsync(Url, Output);
            return 0;
        }
    }
}
