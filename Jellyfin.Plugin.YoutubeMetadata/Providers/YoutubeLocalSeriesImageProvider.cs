using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Jellyfin.Plugin.YoutubeMetadata.Providers;

/// <summary>
/// Provides local series image information from YouTube.
/// </summary>
public class YoutubeLocalSeriesImageProvider(IFileSystem fileSystem) : ILocalImageProvider
{
    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name => Constants.PluginName;

    /// <summary>
    /// Retrieves a list of local images for a given item.
    /// </summary>
    /// <param name="item">The base item for which to retrieve the images.</param>
    /// <param name="directoryService">The directory service used to access the file system.</param>
    /// /// <returns>A collection of local image information.</returns>
    public IEnumerable<LocalImageInfo> GetImages(BaseItem item, IDirectoryService directoryService)
    {
        var list = new List<LocalImageInfo>();
        string jpgPath = GetSeriesInfo(item.Path);
        if (string.IsNullOrEmpty(jpgPath))
        {
            return list;
        }

        var localimg = new LocalImageInfo();
        var fileInfo = fileSystem.GetFileSystemInfo(jpgPath);
        localimg.FileInfo = fileInfo;
        list.Add(localimg);
        return list;
    }

    /// <summary>
    /// Determines whether the provider supports the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns><c>true</c> if the provider supports the item; otherwise, <c>false</c>.</returns>
    public bool Supports(BaseItem item) => item is Series;

    private string GetSeriesInfo(string path)
    {
        Matcher matcher = new();
        matcher.AddInclude("**/*.jpg");
        matcher.AddInclude("**/*.webp");
        return matcher.GetResultsInFullPath(path).Where((file) => Constants.youtubeChannelRegex().Match(file).Success).FirstOrDefault(defaultValue: null) ?? throw new FileNotFoundException("Series info not found.");
        throw new FileNotFoundException("No series image found.");
    }
}
