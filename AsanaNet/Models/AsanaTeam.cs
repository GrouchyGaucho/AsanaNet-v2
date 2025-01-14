using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaTeam
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("organization")]
    public AsanaWorkspace? Organization { get; set; }
} 