using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
} 