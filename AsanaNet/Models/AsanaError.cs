using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents an error returned by the Asana API.
/// </summary>
public class AsanaError
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional help text for resolving the error.
    /// </summary>
    [JsonPropertyName("help")]
    public string? Help { get; set; }

    /// <summary>
    /// Gets or sets a short phrase describing the error type.
    /// </summary>
    [JsonPropertyName("phrase")]
    public string? Phrase { get; set; }
} 