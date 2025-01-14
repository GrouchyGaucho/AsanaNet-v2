using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a request to create a task in Asana.
/// </summary>
public class AsanaTaskCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the task.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the workspace ID where the task will be created.
    /// </summary>
    [JsonPropertyName("workspace")]
    public string WorkspaceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of project IDs to add the task to.
    /// </summary>
    [JsonPropertyName("projects")]
    public List<string>? ProjectIds { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user to assign the task to.
    /// </summary>
    [JsonPropertyName("assignee")]
    public string? AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the due date of the task.
    /// </summary>
    [JsonPropertyName("due_on")]
    public DateTime? DueOn { get; set; }

    /// <summary>
    /// Gets or sets the exact due date and time of the task.
    /// </summary>
    [JsonPropertyName("due_at")]
    public DateTime? DueAt { get; set; }

    /// <summary>
    /// Gets or sets the list of user IDs who should follow the task.
    /// </summary>
    [JsonPropertyName("followers")]
    public List<string>? FollowerIds { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent task if this is a subtask.
    /// </summary>
    [JsonPropertyName("parent")]
    public string? ParentTaskId { get; set; }

    /// <summary>
    /// Gets or sets the list of tag IDs to apply to the task.
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string>? TagIds { get; set; }

    /// <summary>
    /// Gets or sets the start date of the task.
    /// </summary>
    [JsonPropertyName("start_on")]
    public DateTime? StartOn { get; set; }

    /// <summary>
    /// Gets or sets the custom field values for the task.
    /// </summary>
    [JsonPropertyName("custom_fields")]
    public Dictionary<string, object>? CustomFields { get; set; }

    /// <summary>
    /// Gets or sets the list of task IDs that this task depends on.
    /// </summary>
    [JsonPropertyName("dependencies")]
    public List<string>? DependencyIds { get; set; }
} 