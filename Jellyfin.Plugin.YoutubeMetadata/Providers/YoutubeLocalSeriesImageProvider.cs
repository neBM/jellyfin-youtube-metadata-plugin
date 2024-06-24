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
    public IEnumerable<LocalImageInfo> GetImages(BaseItem item, IDirectoryService directoryService) => new List<string> { ".jpg", ".webp" }
            .Select((ext) => Path.Join(item.Path, Path.GetFileName(item.Path) + ext))
            .Where(fileSystem.FileExists)
            .Select((path) => new LocalImageInfo { FileInfo = fileSystem.GetFileSystemInfo(path) });

    /// <summary>
    /// Determines whether the provider supports the specified item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns><c>true</c> if the provider supports the item; otherwise, <c>false</c>.</returns>
    public bool Supports(BaseItem item) => item is Series;
}
