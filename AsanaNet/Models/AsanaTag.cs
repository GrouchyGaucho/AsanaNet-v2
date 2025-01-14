using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaTag
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("followers")]
    public List<AsanaUser> Followers { get; set; } = new();

    [JsonPropertyName("workspace")]
    public AsanaWorkspace? Workspace { get; set; }
} 