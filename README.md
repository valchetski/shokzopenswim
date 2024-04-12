# Shokz OpenSwim downloader
[![NuGet](https://buildstats.info/nuget/Shokz)](https://www.nuget.org/packages/Shokz)

Tool to download Podcasts from RSS feed to Shokz OpenSwim headphones.
It does the transmission of the files in a specific order as it is an important criterion for playback sequence on [OpenSwim](https://intl.help.shokz.com/s/article/How-to-list-the-track-order-on-OpenSwim-formerly-Xtrainerz-17).

# Installation
Install .Net tool:
```
dotnet tool install --global Shokz
```

Verify the installation:
```
shokz --help
```

# Usage
1. Make sure you plug in your OpenSwim headphones to your machine via USB.
2. Run the [shokz](#shokz) command in the terminal with parameters to download Podcast to OpenSwim.

# Commands
## shokz
```
shokz [<URI>] [-o|--output <OUTPUT>]
```
### Arguments
- `URI`\
One of the values:
    - URL to RSS feed with Podcast.
    - Path to a local folder with downloaded Podcast.

### Options
- `-o|--output`\
Root path to download podcasts.\
Default values:
    - Mac: `/Volumes/OpenSwim`
    - Other platforms: not specified

### Examples
#### RSS feed without `-o|--output`
```
shokz https://valchetski.github.io/shokzopenswim/samplerss.xml
```

#### RSS feed with `-o|--output`
```
shokz https://valchetski.github.io/shokzopenswim/samplerss.xml -o /Volumes/OpenSwim/Podcasts
```

#### Local folder
```
shokz /Users/%yourusername%/Downloads/CoolPodcast
```
