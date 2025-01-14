using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a like/heart on an Asana object.
/// </summary>
public class AsanaHeart
{
    /// <summary>
    /// Gets or sets the unique identifier of the heart.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who created the heart.
    /// </summary>
    [JsonPropertyName("user")]
    public AsanaUser? User { get; set; }

    /// <summary>
    /// Gets or sets when the heart was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
} 