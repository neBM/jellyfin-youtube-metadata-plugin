// <copyright file="YoutubeLocalEpisodeProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Jellyfin.Plugin.YoutubeMetadata.Providers;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Data.Enums;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides local metadata for episodes from YouTube videos.
/// </summary>
public class YoutubeLocalEpisodeProvider(IFileSystem fileSystem, ILogger logger) : ILocalMetadataProvider<Episode>
{
    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public string Name => Constants.PluginName;

    /// <summary>
    /// Retrieves the metadata for an episode from a local YouTube video file.
    /// </summary>
    /// <param name="info">The information about the video file.</param>
    /// <param name="directoryService">The directory service used to access the file system.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the metadata for the episode.</returns>
    public Task<MetadataResult<Episode>> GetMetadata(ItemInfo info, IDirectoryService directoryService, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(info, nameof(info));

        var infoJsonPath = Path.ChangeExtension(info.Path, "info.json");
        if (!fileSystem.FileExists(infoJsonPath))
        {
            logger.LogError("info.json file not found: {InfoJsonPath}", infoJsonPath);
            throw new FileNotFoundException("info.json file not found", infoJsonPath);
        }

        var infoJson = Utils.ReadYTDLInfo(infoJsonPath);

        logger.LogInformation("Retrieving metadata for episode: {EpisodeId}", infoJson.Id);

        var episode = new MetadataResult<Episode>
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
                Name = infoJson.Title,
                Overview = infoJson.Description,
                PremiereDate = DateTime.ParseExact(infoJson.UploadDate, "yyyyMMdd", null),
                IndexNumber = infoJson.PlaylistIndex,
                ParentIndexNumber = 1,
                ProviderIds = new() { { Constants.PluginName, infoJson.Id } },
            },
        };

        logger.LogInformation("Metadata retrieved for episode: {EpisodeId}", infoJson.Id);

        return Task.FromResult(episode);
    }
}
