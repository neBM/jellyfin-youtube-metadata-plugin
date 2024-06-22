using System.IO;
using System.Text.Json;

namespace Jellyfin.Plugin.YoutubeMetadata
{
    internal static class Utils
    {
        internal static YTDLData ReadYTDLInfo(string infoJsonPath) => JsonSerializer.Deserialize<YTDLData>(File.ReadAllText(infoJsonPath)) ?? throw new JsonException("Failed to deserialize info.json");
    }
}
