using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaProject
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; set; }

    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("current_status")]
    public AsanaProjectStatus? CurrentStatus { get; set; }

    [JsonPropertyName("public")]
    public bool IsPublic { get; set; }

    [JsonPropertyName("team")]
    public AsanaTeam? Team { get; set; }

    [JsonPropertyName("workspace")]
    public AsanaWorkspace? Workspace { get; set; }

    [JsonPropertyName("followers")]
    public List<AsanaUser> Followers { get; set; } = new();
}

public class AsanaProjectStatus
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public AsanaUser? Author { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
} 