using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
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

    public new class Handler(IServiceProvider services, ILogger<Handler> logger) : ICommandHandler
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
                throw new InvalidOperationException($"\"{nameof(Uri)}\" cannot be null");
            }

            if (Output == null)
            {
                throw new InvalidOperationException($"\"{nameof(Output)}\" cannot be null");
            }

            IFeedReader? feedReader = Uri switch
            {
                string when System.Uri.TryCreate(Uri, UriKind.Absolute, out Uri? uriResult) 
                    && (uriResult.Scheme == System.Uri.UriSchemeHttp || uriResult.Scheme == System.Uri.UriSchemeHttps) 
                    => services.GetRequiredService<RssFeedReader>(),
                string when Path.Exists(Uri) => services.GetRequiredService<LocalFeedReader>(),
                _ => null
            };

            if (feedReader == null)
            {
                logger.LogError("No feed found at \"{Uri}\".", Uri);
                return 1;
            }

            await feedReader.DownloadAsync(Uri, Output);
            return 0;
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
