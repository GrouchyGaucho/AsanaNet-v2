using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents an enumeration option for a custom field in Asana.
/// </summary>
public class AsanaEnumOption
{
    /// <summary>
    /// Gets or sets the unique identifier of the enum option.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the enum option.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the enum option is enabled.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the color of the enum option.
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; set; }
} 