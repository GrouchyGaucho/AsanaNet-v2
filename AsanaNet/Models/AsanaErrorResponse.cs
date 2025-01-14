using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models
{
    public class AsanaErrorResponse
    {
        [JsonPropertyName("errors")]
        public List<AsanaError> Errors { get; set; } = new();
    }

    public class AsanaError
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
} 