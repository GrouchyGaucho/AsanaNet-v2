using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaWorkspace
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("is_organization")]
    public bool IsOrganization { get; set; }
} 