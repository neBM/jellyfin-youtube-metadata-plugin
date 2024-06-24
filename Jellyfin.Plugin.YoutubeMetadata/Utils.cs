using System.IO;
using System.Text.Json;

namespace Jellyfin.Plugin.YoutubeMetadata
{
    internal static class Utils
    {
        internal static YTVideoDto ReadYTVideoInfo(string infoJsonPath) => JsonSerializer.Deserialize<YTVideoDto>(File.ReadAllText(infoJsonPath)) ?? throw new JsonException("Failed to deserialize info.json");

        internal static YTChannelDto ReadYTChannelInfo(string infoJsonPath) => JsonSerializer.Deserialize<YTChannelDto>(File.ReadAllText(infoJsonPath)) ?? throw new JsonException("Failed to deserialize info.json");
    }
}
