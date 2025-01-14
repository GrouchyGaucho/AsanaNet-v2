using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaTask
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("completed")]
    public bool IsCompleted { get; set; }

    [JsonPropertyName("assignee")]
    public AsanaUser? Assignee { get; set; }

    [JsonPropertyName("due_on")]
    public DateTime? DueOn { get; set; }

    [JsonPropertyName("due_at")]
    public DateTime? DueAt { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; set; }

    [JsonPropertyName("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [JsonPropertyName("workspace")]
    public AsanaWorkspace? Workspace { get; set; }

    [JsonPropertyName("projects")]
    public List<AsanaProject> Projects { get; set; } = new();

    [JsonPropertyName("tags")]
    public List<AsanaTag> Tags { get; set; } = new();

    [JsonPropertyName("followers")]
    public List<AsanaUser> Followers { get; set; } = new();
} 