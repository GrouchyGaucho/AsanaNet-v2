using System;
using System.Collections.Generic;

namespace AsanaNet.Models;

public class AsanaEvents
{
    public string? Sync { get; set; }
    public List<AsanaEventItem>? Data { get; set; }
} 