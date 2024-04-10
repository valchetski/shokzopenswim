using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Shokz.Tests;

public class RssFeedReaderTests
{
    [Fact]
    public async Task Download()
    {
        // arrange
        var downloadLocation = "Blackout";
        Directory.Delete(downloadLocation);

        var loggerStub = new Mock<ILogger<RssFeedReader>>();

        var sut = new RssFeedReader(loggerStub.Object);

        // act
        var act = async () => await sut.DownloadAsync("https://feeds.megaphone.fm/QCEOS5292368649", downloadLocation);

        // assert
        await act.Should().NotThrowAsync();
        Directory.Delete(downloadLocation);
    }
}
