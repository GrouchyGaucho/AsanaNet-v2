using System;
using AsanaNet.Enums;

namespace AsanaNet.Options;

public class AsanaOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Basic;
    public Action<string, string, string>? ErrorCallback { get; set; }
} 