using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaCustomField
{
    [JsonPropertyName("gid")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("resource_type")]
    public string ResourceType { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("enum_options")]
    public List<AsanaEnumOption>? EnumOptions { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("enum_value")]
    public AsanaEnumOption? EnumValue { get; set; }

    [JsonPropertyName("text_value")]
    public string? TextValue { get; set; }

    [JsonPropertyName("number_value")]
    public decimal? NumberValue { get; set; }

    [JsonPropertyName("date_value")]
    public AsanaDate? DateValue { get; set; }

    [JsonPropertyName("multi_enum_values")]
    public List<AsanaEnumOption>? MultiEnumValues { get; set; }
} 