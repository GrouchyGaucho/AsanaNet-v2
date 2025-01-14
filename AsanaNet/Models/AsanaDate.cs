using System;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

/// <summary>
/// Represents a date value in Asana that can be either a date or a datetime.
/// </summary>
public class AsanaDate
{
    /// <summary>
    /// Gets or sets the date value (without time).
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or sets the datetime value (with time).
    /// </summary>
    [JsonPropertyName("datetime")]
    public DateTime? DateTime { get; set; }
} 