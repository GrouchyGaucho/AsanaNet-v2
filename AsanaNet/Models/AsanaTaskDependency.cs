using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a dependency relationship between tasks in Asana.
/// </summary>
public class AsanaTaskDependency
{
    /// <summary>
    /// Gets or sets the unique identifier of the dependency.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the task that must be completed first.
    /// </summary>
    [JsonPropertyName("target")]
    public AsanaTask Target { get; set; } = new();

    /// <summary>
    /// Gets or sets the task that depends on the target task.
    /// </summary>
    [JsonPropertyName("dependant")]
    public AsanaTask Dependant { get; set; } = new();
} 