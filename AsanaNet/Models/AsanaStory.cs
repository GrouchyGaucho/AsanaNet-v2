using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a story (comment or system-generated update) on a task in Asana.
/// </summary>
public class AsanaStory
{
    /// <summary>
    /// Gets or sets the unique identifier of the story.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the story.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the story was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the user who created the story.
    /// </summary>
    [JsonPropertyName("created_by")]
    public AsanaUser? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the type of story (comment, system, etc.).
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
} 