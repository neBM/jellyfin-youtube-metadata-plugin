using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Providers;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Jellyfin.Plugin.YoutubeMetadata.Providers;

/// <summary>
/// Provides extension methods for the <see cref="IDirectoryService"/> interface.
/// </summary>
internal static partial class DirectoryServiceExtensions
{
    public static IEnumerable<string> GetSeriesInfo(this IDirectoryService directoryService, string path)
    {
        Matcher matcher = new();
        matcher.AddInclude("*.jpg");
        matcher.AddInclude("*.webp");
        return matcher.GetResultsInFullPath(path).Where((file) => Constants.youtubeIdRegex().Match(file).Success);
    }
}
