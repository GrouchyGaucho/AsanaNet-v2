using System.Text.Json.Serialization;

namespace AsanaNet.Models
{
    public class AsanaUser
    {
        [JsonPropertyName("gid")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
} 