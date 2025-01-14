using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a task's membership within a project and section.
/// </summary>
public class AsanaTaskMembership
{
    /// <summary>
    /// Gets or sets the unique identifier of the membership.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the project containing the task.
    /// </summary>
    [JsonPropertyName("project")]
    public AsanaProject Project { get; set; } = new();

    /// <summary>
    /// Gets or sets the section containing the task, if any.
    /// </summary>
    [JsonPropertyName("section")]
    public AsanaSection? Section { get; set; }
} 