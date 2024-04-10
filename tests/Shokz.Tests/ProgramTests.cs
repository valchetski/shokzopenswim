using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shokz.Tests;

public class ProgramTests
{
    [Fact]
    public void Main()
    {
        // arrange
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => {
                builder.UseSetting("Url", "https://feeds.megaphone.fm/QCEOS5292368649");
                builder.UseSetting("Location", "Blackout");
            });
        var host = application.Services.GetRequiredService<IHost>();

        // act
        var act = () => host.WaitForShutdown();

        // assert
        act.Should().NotThrow();
    }
}
