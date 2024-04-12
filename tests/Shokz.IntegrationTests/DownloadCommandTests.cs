using System.CommandLine;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shokz.IntegrationTests;

public class DownloadCommandTests
{
    private readonly Parser _parser;
    private readonly IConsole _console;

    public DownloadCommandTests()
    {
        _console = new TestConsole();
        _parser = CommandLine.CreateParser(x => 
        {
            x.RemoveAll<IConsole>();
            x.AddSingleton(_console);
        });
    }

    [Fact]
    public async Task NoArguments_ShouldShowError()
    {
        // arrange
        // act
        var result = await InvokeAsync();

        // assert
        result.Should().Be(1);
        _console.Error.ToString().Should().Contain("Required argument missing for command");
    }

    [Fact]
    public async Task InvalidUriProvided_ShouldShowError()
    {
        // arrange
        var invalidUri = "i am invalid";

        // act
        var result = await InvokeAsync(invalidUri);

        // assert
        result.Should().Be(1);
        _console.Error.ToString().Should().Contain("No feed found at")
            .And.Contain(invalidUri);
    }

    [Fact]
    public async Task CorrectRssFeedUrl_ShouldDownload()
    {
        // arrange
        // act
        var result = await InvokeAsync("https://valchetski.github.io/shokzopenswim/samplerss.xml", "-o=samplerss");

        // assert
        result.Should().Be(0, _console.Error.ToString());
    }

    [Fact]
    public async Task CorrectLocalFolder_ShouldDownload()
    {
        // arrange
        // act
        var result = await InvokeAsync("TestsInfrastructure/Feed", "-o=samplelocal");

        // assert
        result.Should().Be(0, _console.Error.ToString());
    }

    private Task<int> InvokeAsync(params string[] args)
    {
        return _parser.InvokeAsync(args, _console);
    }
}