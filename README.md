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
shokz [<URL>] [--output <OUTPUT>]
```
### Arguments
- `URL`\
URL to RSS feed with Podcast

### Options
- `--output`\
Root path to download podcasts.\
Default values:
    - Mac: `/Volumes/OpenSwim`
    - Other platforms: not specified

### Examples
#### Without --output
```
shokz https://feeds.megaphone.fm/QCEOS5292368649
```

#### With --output
```
shokz https://feeds.megaphone.fm/QCEOS5292368649 --output /Volumes/OpenSwim/Podcasts
```

