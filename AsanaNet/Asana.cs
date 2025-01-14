using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using AsanaNet.Interfaces;
using AsanaNet.Options;
using AsanaNet.Enums;
using AsanaNet.Models;

namespace AsanaNet;

[Serializable]
public class Asana : IAsanaClient
{
    private readonly HttpClient _httpClient;
    private Action<Exception>? _errorCallback;

    public event Action<Exception> OnError
    {
        add => _errorCallback += value;
        remove => _errorCallback -= value;
    }

    public string ApiKey { get; init; }
    public AuthenticationType AuthType { get; }
    public string? OAuthToken { get; private set; }
    public string BaseUrl { get; set; } = "https://app.asana.com/api/1.0/";

    public Asana(string apiKeyOrBearerToken, AuthenticationType authType, Action<Exception>? errorCallback = null)
    {
        ApiKey = apiKeyOrBearerToken;
        AuthType = authType;
        _errorCallback = errorCallback;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKeyOrBearerToken);
    }

    public async Task<AsanaUser?> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<AsanaUser>>("users/me", cancellationToken);
        return response?.Data;
    }

    public async Task<AsanaObjectCollection> GetWorkspacesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaWorkspace>>>("workspaces", cancellationToken);
        return new AsanaObjectCollection { Data = response?.Data?.Cast<object>().ToList() ?? new List<object>() };
    }

    public async Task<AsanaObjectCollection> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTeam>>>($"organizations/{workspace.Id}/teams", cancellationToken);
        return new AsanaObjectCollection { Data = response?.Data?.Cast<object>().ToList() ?? new List<object>() };
    }

    public async Task<AsanaObjectCollection> GetProjectsInTeamAsync(AsanaTeam team, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaProject>>>($"teams/{team.Id}/projects", cancellationToken);
        return new AsanaObjectCollection { Data = response?.Data?.Cast<object>().ToList() ?? new List<object>() };
    }

    public async Task<AsanaObjectCollection> GetTasksInAProjectAsync(AsanaProject project, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTask>>>($"projects/{project.Id}/tasks", cancellationToken);
        return new AsanaObjectCollection { Data = response?.Data?.Cast<object>().ToList() ?? new List<object>() };
    }

    public async Task<AsanaEvents> GetEventsInAProjectAsync(long projectId, string? lastToken = null, CancellationToken cancellationToken = default)
    {
        var url = $"projects/{projectId}/events" + (lastToken != null ? $"?sync={lastToken}" : "");
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<AsanaEvents>>(url, cancellationToken);
        return response?.Data ?? new AsanaEvents();
    }

    private void ErrorCallback(Exception ex)
    {
        _errorCallback?.Invoke(ex);
    }
}
