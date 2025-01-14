using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a tag in Asana used to categorize and organize tasks.
/// </summary>
public class AsanaTag
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the tag.
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the description of the tag.
    /// </summary>
    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the workspace containing this tag.
    /// </summary>
    [JsonPropertyName("workspace")]
    public AsanaWorkspace Workspace { get; set; } = new();
} 