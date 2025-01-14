using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a task in Asana.
/// </summary>
public class AsanaTask
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the task.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the resource type.
    /// </summary>
    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the task is completed.
    /// </summary>
    [JsonPropertyName("completed")]
    public bool Completed { get; set; }

    /// <summary>
    /// Gets or sets when the task was completed.
    /// </summary>
    [JsonPropertyName("completed_at")]
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets when the task was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when the task was last modified.
    /// </summary>
    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the date when the task is due.
    /// </summary>
    [JsonPropertyName("due_on")]
    public DateTime? DueOn { get; set; }

    /// <summary>
    /// Gets or sets the exact date and time when the task is due.
    /// </summary>
    [JsonPropertyName("due_at")]
    public DateTime? DueAt { get; set; }

    /// <summary>
    /// Gets or sets the date when the task should start.
    /// </summary>
    [JsonPropertyName("start_on")]
    public DateTime? StartOn { get; set; }

    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user assigned to the task.
    /// </summary>
    [JsonPropertyName("assignee")]
    public AsanaUser? Assignee { get; set; }

    /// <summary>
    /// Gets or sets the parent task if this is a subtask.
    /// </summary>
    [JsonPropertyName("parent")]
    public AsanaTask? Parent { get; set; }

    /// <summary>
    /// Gets or sets the workspace containing this task.
    /// </summary>
    [JsonPropertyName("workspace")]
    public AsanaWorkspace? Workspace { get; set; }

    /// <summary>
    /// Gets or sets the number of subtasks this task has.
    /// </summary>
    [JsonPropertyName("num_subtasks")]
    public int NumberOfSubtasks { get; set; }

    /// <summary>
    /// Gets or sets the user who completed the task.
    /// </summary>
    [JsonPropertyName("completed_by")]
    public AsanaUser? CompletedBy { get; set; }

    /// <summary>
    /// Gets or sets the list of users following this task.
    /// </summary>
    [JsonPropertyName("followers")]
    public List<AsanaUser> Followers { get; set; } = new();

    /// <summary>
    /// Gets or sets the tags associated with this task.
    /// </summary>
    [JsonPropertyName("tags")]
    public List<AsanaTag> Tags { get; set; } = new();

    /// <summary>
    /// Gets or sets the projects containing this task.
    /// </summary>
    [JsonPropertyName("projects")]
    public List<AsanaProject> Projects { get; set; } = new();

    /// <summary>
    /// Gets or sets the custom fields and their values for this task.
    /// </summary>
    [JsonPropertyName("custom_fields")]
    public List<AsanaCustomField> CustomFields { get; set; } = new();

    /// <summary>
    /// Gets or sets the project and section memberships of this task.
    /// </summary>
    [JsonPropertyName("memberships")]
    public List<AsanaTaskMembership> Memberships { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the current user has liked this task.
    /// </summary>
    [JsonPropertyName("hearted")]
    public bool Hearted { get; set; }

    /// <summary>
    /// Gets or sets the list of likes on this task.
    /// </summary>
    [JsonPropertyName("hearts")]
    public List<AsanaHeart> Hearts { get; set; } = new();

    /// <summary>
    /// Gets or sets the number of likes this task has received.
    /// </summary>
    [JsonPropertyName("num_hearts")]
    public int NumberOfHearts { get; set; }

    /// <summary>
    /// Gets or sets the tasks that this task depends on.
    /// </summary>
    [JsonPropertyName("dependencies")]
    public List<AsanaTask> Dependencies { get; set; } = new();

    /// <summary>
    /// Gets or sets the tasks that depend on this task.
    /// </summary>
    [JsonPropertyName("dependents")]
    public List<AsanaTask> Dependents { get; set; } = new();
} 