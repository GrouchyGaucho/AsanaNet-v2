using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaUser
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("photo")]
    public AsanaUserPhoto? Photo { get; set; }
}

public class AsanaUserPhoto
{
    [JsonPropertyName("image_21x21")]
    public string? Image21x21 { get; set; }

    [JsonPropertyName("image_27x27")]
    public string? Image27x27 { get; set; }

    [JsonPropertyName("image_36x36")]
    public string? Image36x36 { get; set; }

    [JsonPropertyName("image_60x60")]
    public string? Image60x60 { get; set; }

    [JsonPropertyName("image_128x128")]
    public string? Image128x128 { get; set; }
} 