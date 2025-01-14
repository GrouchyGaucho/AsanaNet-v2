using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a file attachment in Asana.
/// </summary>
public class AsanaAttachment
{
    /// <summary>
    /// Gets or sets the unique identifier of the attachment.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the attachment.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the attachment was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the URL where the attachment can be downloaded.
    /// </summary>
    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the host service of the attachment.
    /// </summary>
    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parent task this attachment belongs to.
    /// </summary>
    [JsonPropertyName("parent")]
    public AsanaTask? Parent { get; set; }

    /// <summary>
    /// Gets or sets the URL where the attachment can be viewed.
    /// </summary>
    [JsonPropertyName("view_url")]
    public string ViewUrl { get; set; } = string.Empty;
} 