using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaResponse<T>
{
    [JsonPropertyName("data")]
    public required T Data { get; set; }
} 