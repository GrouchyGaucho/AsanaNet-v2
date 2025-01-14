using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaObjectCollection<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();
}

// For backward compatibility
public class AsanaObjectCollection : AsanaObjectCollection<object>
{
} 