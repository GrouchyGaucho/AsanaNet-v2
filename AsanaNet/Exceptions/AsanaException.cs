using System;

namespace AsanaNet.Exceptions;

public class AsanaException : Exception
{
    public AsanaException(string message) : base(message) { }
    public AsanaException(string message, Exception innerException) : base(message, innerException) { }
} 