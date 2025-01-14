using System.Text.Json.Serialization;

namespace AsanaNet.Models
{
    public class AsanaTeam
    {
        [JsonPropertyName("gid")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
} 