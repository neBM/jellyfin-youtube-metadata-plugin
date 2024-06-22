using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.IO;

namespace Jellyfin.Plugin.YoutubeMetadata.Providers
{
    /// <summary>
    /// Provides local images for YouTube videos.
    /// </summary>
    public class YoutubeLocalImageProvider(IFileSystem fileSystem) : ILocalImageProvider
    {
        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public string Name => Constants.PluginName;

        /// <summary>
        /// Gets the local images for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="directoryService">The directory service.</param>
        /// <returns>The local images.</returns>
        public IEnumerable<LocalImageInfo> GetImages(BaseItem item, IDirectoryService directoryService) => directoryService
            .GetSeriesInfo(item.ContainingFolderPath)
            .Select((jpgPath) => new LocalImageInfo
            {
                Type = ImageType.Primary,
                FileInfo = fileSystem.GetFileSystemInfo(jpgPath),
            });

        /// <summary>
        /// Determines whether the provider supports the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if the provider supports the item; otherwise, <c>false</c>.</returns>
        public bool Supports(BaseItem item) => item is Episode;
    }
}
