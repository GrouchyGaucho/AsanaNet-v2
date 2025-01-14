using System;
using System.Threading.Tasks;

namespace AsanaNet;

/// <summary>
/// Interface defining the contract for interacting with the Asana API
/// </summary>
public interface IAsanaClient
{
    /// <summary>
    /// Event that is raised when an error occurs during API operations
    /// </summary>
    event Action<Exception> OnError;

    /// <summary>
    /// Gets the API key used for authentication
    /// </summary>
    string ApiKey { get; }

    /// <summary>
    /// Gets or sets the base URL for the Asana API
    /// </summary>
    string BaseUrl { get; set; }
} 