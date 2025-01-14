using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a value for a custom field in Asana.
/// </summary>
public class AsanaCustomFieldValue
{
    /// <summary>
    /// Gets or sets the ID of the custom field.
    /// </summary>
    [JsonPropertyName("gid")]
    public string CustomFieldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text value for text custom fields.
    /// </summary>
    [JsonPropertyName("text_value")]
    public string? TextValue { get; set; }

    /// <summary>
    /// Gets or sets the number value for number custom fields.
    /// </summary>
    [JsonPropertyName("number_value")]
    public decimal? NumberValue { get; set; }

    /// <summary>
    /// Gets or sets the enum option ID for enum custom fields.
    /// </summary>
    [JsonPropertyName("enum_value")]
    public string? EnumValueId { get; set; }

    /// <summary>
    /// Gets or sets the enum option IDs for multi-enum custom fields.
    /// </summary>
    [JsonPropertyName("multi_enum_values")]
    public List<string>? MultiEnumValueIds { get; set; }
} 