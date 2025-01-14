namespace AsanaNet.Options
{
    public class AsanaOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public AuthenticationType AuthenticationType { get; set; } = AuthenticationType.Basic;
    }
} 