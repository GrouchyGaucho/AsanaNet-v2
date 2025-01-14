using System.Text.Json.Serialization;

namespace AsanaNet.Models
{
    public class AsanaTask
    {
        [JsonPropertyName("gid")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }

    public class AsanaTaskCreateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;

        [JsonPropertyName("workspace")]
        public string WorkspaceId { get; set; } = string.Empty;
    }

    public class AsanaTaskUpdateRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = string.Empty;
    }
} 