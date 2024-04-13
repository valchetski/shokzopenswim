using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shokz;

public class FeedProcessor(
    IFeedDownloader feedDownloader,
    ILogger<FeedProcessor> logger,
    IServiceProvider services) : IFeedProcessor
{
    public async Task DownloadAsync(string uri, string downloadLocation, string? splitDuration = null)
    {
        IFeedReader? feedReader = UriUtil.GetUriType(uri) switch
        {
            UriType.Http => services.GetRequiredService<RssFeedReader>(),
            UriType.Local => services.GetRequiredService<LocalFeedReader>(),
            _ => throw new FeedException($"No feed found at \"{uri}\".")
        };
        var feed = await feedReader.GetFeedAsync(uri);
        var downloadedFiles = await feedDownloader.DownloadAsync(feed, downloadLocation);
        if (!string.IsNullOrEmpty(splitDuration) && downloadedFiles.Length > 0)
        {
            SplitFiles(downloadedFiles, splitDuration);
        }
    }

    private void SplitFiles(string[] files, string splitDuration)
    {
        TimeSpan duration;
        if (splitDuration.EndsWith('s'))
        {
            duration = TimeSpan.FromSeconds(int.Parse(splitDuration.TrimEnd('s')));
            logger.LogInformation("Tracks will be splitted in {Duration} seconds parts.", duration.TotalSeconds);
        }
        else if (splitDuration.EndsWith('m'))
        {
            duration = TimeSpan.FromMinutes(int.Parse(splitDuration.TrimEnd('m')));
            logger.LogInformation("Tracks will be splitted in {Duration} minutes parts.", duration.TotalMinutes);
        }
        else
        {
            duration = TimeSpan.FromMinutes(int.Parse(splitDuration));
            logger.LogInformation("Tracks will be splitted in {Duration} minutes parts.", duration.TotalMinutes);
        }

        foreach (var file in files) 
        {
            SplitMp3File(file, duration);
        }
        logger.LogInformation("Splitting of tracks completed");
    }

    private void SplitMp3File(string file, TimeSpan duration)
    {
        logger.LogInformation("Splitting \"{File}\" file.", file);
        var directory = Path.GetDirectoryName(file);
        var fileName = Path.GetFileNameWithoutExtension(file);
        var extension = Path.GetExtension(file);
        if (directory == null)
        {
            throw new FeedException($"Can't find directory for \"{file}\"");
        }
        var outputPathPattern = Path.Combine(directory, $"{fileName}_%03d{extension}");

        var process = new Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = $"-i \"{file}\" -f segment -segment_time \"{duration.TotalSeconds}\" -c copy \"{outputPathPattern}\"";
        process.Start();
        process.WaitForExit();

        logger.LogInformation("Splitted \"{File}\" file.", file);

        logger.LogInformation("Deleting \"{File}\" file.", file);
        File.Delete(file);
        logger.LogInformation("Deleted \"{File}\" file.", file);
    }

    private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) 
    {
        logger.LogInformation(outLine.Data);
    }
}
