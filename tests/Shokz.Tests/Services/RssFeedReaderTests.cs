using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Shokz.Tests;

public class RssFeedReaderTests
{
    [Fact]
    public async Task Download()
    {
        // arrange
        var downloadLocation = "Sample";
        if (Directory.Exists(downloadLocation))
        {
            Directory.Delete(downloadLocation, true);
        }

        var services = new ServiceCollection()
            .AddFeedServices()
            .AddLogging()
            .BuildServiceProvider();

        var sut = services.GetRequiredService<IFeedProcessor>();

        // act
        var act = async () => await sut.DownloadAsync("https://valchetski.github.io/shokzopenswim/samplerss.xml", downloadLocation);

        // assert
        await act.Should().NotThrowAsync();
        Directory.Delete(downloadLocation, true);
    }
}
