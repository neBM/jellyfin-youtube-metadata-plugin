using System;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.YoutubeMetadata
{
    /// <summary>
    /// Contains constants used by the plugin.
    /// </summary>
    internal sealed partial class Constants
    {
        internal const string PluginName = "YoutubeMetadata";
        internal static readonly Guid PluginGuid = Guid.Parse("4c748daa-a7e4-4ed1-817c-5e18c683585e");
    }
}
