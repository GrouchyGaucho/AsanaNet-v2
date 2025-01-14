using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a section within a project in Asana.
/// </summary>
public class AsanaSection
{
    /// <summary>
    /// Gets or sets the unique identifier of the section.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the section.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the project containing this section.
    /// </summary>
    [JsonPropertyName("project")]
    public AsanaProject Project { get; set; } = new();
} 