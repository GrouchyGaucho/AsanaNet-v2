using System;

namespace AsanaNet.Models;

public class AsanaEventItem
{
    public string Type { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public AsanaEventResource? Resource { get; set; }
}

public class AsanaEventResource
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public AsanaUser? CreatedBy { get; set; }
} 