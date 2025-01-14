using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Base class for all Asana objects
/// </summary>
public class AsanaObject
{
    /// <summary>
    /// Gets or sets the unique identifier of the object
    /// </summary>
    [JsonPropertyName("gid")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the resource type of the object
    /// </summary>
    [JsonPropertyName("resource_type")]
    public required string ResourceType { get; set; }
} 