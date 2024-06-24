using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.YoutubeMetadata.Providers;

/// <summary>
/// Represents a local metadata provider for YouTube series.
/// </summary>
public class YoutubeLocalSeriesProvider(IFileSystem fileSystem, ILogger<YoutubeLocalSeriesProvider> logger) : ILocalMetadataProvider<Series>
{
    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name => Constants.PluginName;

    /// <summary>
    /// Retrieves the metadata for a series based on the provided item information.
    /// </summary>
    /// <param name="info">The item information.</param>
    /// <param name="directoryService">The directory service.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the metadata for the series.</returns>
    public Task<MetadataResult<Series>> GetMetadata(ItemInfo info, IDirectoryService directoryService, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(info, nameof(info));

        var infoJsonPath = Path.Join(info.Path, Path.GetFileName(info.Path) + ".info.json");
        if (!fileSystem.FileExists(infoJsonPath))
        {
            logger.LogError("info.json file not found: {InfoJsonPath}", infoJsonPath);
            throw new FileNotFoundException("info.json file not found", infoJsonPath);
        }

        var infoJson = Utils.ReadYTChannelInfo(infoJsonPath);

        logger.LogInformation("Retrieving metadata for series: {SeriesId}", infoJson.Id);

        var series = new MetadataResult<Series>
        {
            HasMetadata = true,
            People = [
                new()
                {
                    Name = infoJson.Uploader,
                    Type = PersonKind.Creator,
                    ProviderIds = new() { { Constants.PluginName, infoJson.ChannelId } },
                }
            ],
            Item = new()
            {
                Name = infoJson.Uploader,
                Overview = infoJson.Description,
                ProviderIds = new() { { Constants.PluginName, infoJson.Id } },
            },
        };

        logger.LogInformation("Metadata retrieved for series: {SeriesId}", infoJson.Id);

        return Task.FromResult(series);
    }
}
