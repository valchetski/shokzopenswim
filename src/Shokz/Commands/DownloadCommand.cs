using System.CommandLine;
using System.CommandLine.Invocation;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Shokz;

public class DownloadCommand : RootCommand
{
    private const string _uriArgument = "uri";

    public DownloadCommand()
        : base("Download assets to Shokz OpenSwim")
    {
        AddArgument(new Argument<string>(_uriArgument, "Url to the RSS feed or local folder."));
        AddOption(new Option<string>(["--output", "-o"], GetOutputDefaultValue, "Root path to copy items.")
        {
            IsRequired = true,
        });
    }

    public new class Handler(IFeedProcessor feedProcessor, ILogger<Handler> logger) : ICommandHandler
    {
        public string? Uri { get; set; }
        public string? Output { get; set; }

        public int Invoke(InvocationContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            if (Uri == null)
            {
                logger.LogError("\"{Uri)}\" cannot be null", nameof(Uri));
                return 1;
            }

            if (Output == null)
            {
                logger.LogError("\"{Output)}\" cannot be null", nameof(Output));
                return 1;
            }
            
            try
            {
                await feedProcessor.DownloadAsync(Uri, Output);
                return 0;
            }
            catch(FeedException ex)
            {
                logger.LogError(ex.Message);
                return 1;
            }
        }
    }

    private string GetOutputDefaultValue()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "/Volumes/OpenSwim";
        }
        return string.Empty;
    }
}
