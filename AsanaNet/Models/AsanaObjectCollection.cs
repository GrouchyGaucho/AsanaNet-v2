using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AsanaNet.Models;

public class AsanaObjectCollection : IEnumerable<object>
{
    [JsonPropertyName("data")]
    public List<object> Data { get; set; } = new();

    public IEnumerator<object> GetEnumerator() => Data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
} 