# Shokz OpenSwim downloader
[![NuGet](https://buildstats.info/nuget/Shokz)](https://www.nuget.org/packages/Shokz)

Tool to download Podcasts from RSS feed to Shokz OpenSwim headphones.
It does the transmission of the files in a specific order as it is an important criterion for playback sequence on [OpenSwim](https://intl.help.shokz.com/s/article/How-to-list-the-track-order-on-OpenSwim-formerly-Xtrainerz-17).

# Prerequisites
Need only if you want to use [--split](#shokz) option.

Install `ffmpeg`:
- On Mac:
    ```
    brew install ffmpeg
    ```

# Installation
Install .NET tool:
```
dotnet tool install --global shokz
```

Verify the installation:
```
shokz --help
```

# Update
Update .NET tool:
```
dotnet tool update --global shokz
```

# Usage
1. Make sure you plug in your OpenSwim headphones to your machine via USB.
2. Run the [shokz](#shokz) command in the terminal with parameters to download Podcast to OpenSwim.

# Commands
## shokz
```
shokz [<URI>] [-o|--output <OUTPUT>] [-s|--split <DURATION>]
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
- `-s|--split`\
Duration of the split chunks.\
Duration type is controlled by the value suffix:
    - `m`, no suffix: minutes will be used. For example: `30m` and `30` will be parsed as 30 minutes.
    - `s`: seconds will be used. For example: `30s` will be parsed as 30 seconds.

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

#### Local folder split into 30-minute chunks
```
shokz /Users/%yourusername%/Downloads/CoolPodcast -s 30
```
