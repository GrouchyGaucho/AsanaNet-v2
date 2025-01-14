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
using System.IO;
using AsanaNet.Constants;

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
    public string BaseUrl { get; set; } = AsanaConstants.DefaultBaseUrl;

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
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<AsanaUser>>(
            $"{AsanaConstants.Endpoints.Users}/me", 
            cancellationToken);
        return response?.Data;
    }

    public async Task<AsanaObjectCollection<AsanaWorkspace>> GetWorkspacesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaWorkspace>>>($"{AsanaConstants.Endpoints.Workspaces}", cancellationToken);
        return new AsanaObjectCollection<AsanaWorkspace> { Data = response?.Data ?? new List<AsanaWorkspace>() };
    }

    public async Task<AsanaObjectCollection<AsanaTeam>> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTeam>>>($"{AsanaConstants.Endpoints.Organizations}/{workspace.Id}/{AsanaConstants.Endpoints.Teams}", cancellationToken);
        return new AsanaObjectCollection<AsanaTeam> { Data = response?.Data ?? new List<AsanaTeam>() };
    }

    public async Task<AsanaObjectCollection<AsanaProject>> GetProjectsInTeamAsync(AsanaTeam team, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaProject>>>($"{AsanaConstants.Endpoints.Teams}/{team.Id}/{AsanaConstants.Endpoints.Projects}", cancellationToken);
        return new AsanaObjectCollection<AsanaProject> { Data = response?.Data ?? new List<AsanaProject>() };
    }

    public async Task<AsanaObjectCollection<AsanaTask>> GetTasksInAProjectAsync(AsanaProject project, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTask>>>($"{AsanaConstants.Endpoints.Projects}/{project.Id}/{AsanaConstants.Endpoints.Tasks}", cancellationToken);
        return new AsanaObjectCollection<AsanaTask> { Data = response?.Data ?? new List<AsanaTask>() };
    }

    public async Task<AsanaEvents> GetEventsInAProjectAsync(long projectId, string? lastToken = null, CancellationToken cancellationToken = default)
    {
        var url = $"{AsanaConstants.Endpoints.Projects}/{projectId}/{AsanaConstants.Endpoints.Events}" + (lastToken != null ? $"?sync={lastToken}" : "");
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<AsanaEvents>>(url, cancellationToken);
        return response?.Data ?? new AsanaEvents();
    }

    public async Task<AsanaTask?> GetTaskByIdAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<AsanaTask>>($"{AsanaConstants.Endpoints.Tasks}/{taskId}", cancellationToken);
        return response?.Data;
    }

    public async Task<AsanaTask> CreateTaskAsync(AsanaTaskCreateRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            AsanaConstants.Endpoints.Tasks, 
            new { data = request }, 
            cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaTask>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "creating task");
        return result.Data ?? throw new AsanaException("Task data was null");
    }

    public async Task<AsanaTask> UpdateTaskAsync(string taskId, AsanaTaskUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}", new { data = request }, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaTask>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "updating task");
        return result.Data ?? throw new AsanaException("Task data was null");
    }

    public async Task DeleteTaskAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AsanaObjectCollection<AsanaTask>> GetSubtasksAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTask>>>($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Subtasks}", cancellationToken);
        return new AsanaObjectCollection<AsanaTask> { Data = response?.Data ?? new List<AsanaTask>() };
    }

    public async Task<AsanaObjectCollection<AsanaStory>> GetTaskStoriesAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaStory>>>($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Stories}", cancellationToken);
        return new AsanaObjectCollection<AsanaStory> { Data = response?.Data ?? new List<AsanaStory>() };
    }

    public async Task<AsanaStory> AddCommentToTaskAsync(string taskId, string text, CancellationToken cancellationToken = default)
    {
        var request = new { data = new { text = text } };
        var response = await _httpClient.PostAsJsonAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Stories}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaStory>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "adding comment");
        return result.Data ?? throw new AsanaException("Story data was null");
    }

    public async Task<AsanaObjectCollection<AsanaTaskDependency>> GetTaskDependenciesAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaTaskDependency>>>($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Dependencies}", cancellationToken);
        return new AsanaObjectCollection<AsanaTaskDependency> { Data = response?.Data ?? new List<AsanaTaskDependency>() };
    }

    public async Task AddTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default)
    {
        var request = new { data = new { dependency = dependencyTaskId } };
        var response = await _httpClient.PostAsJsonAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Dependencies}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Dependencies}/{dependencyTaskId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AsanaObjectCollection<AsanaAttachment>> GetTaskAttachmentsAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<AsanaResponse<List<AsanaAttachment>>>($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Attachments}", cancellationToken);
        return new AsanaObjectCollection<AsanaAttachment> { Data = response?.Data ?? new List<AsanaAttachment>() };
    }

    public async Task<AsanaAttachment> UploadAttachmentToTaskAsync(string taskId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        
        content.Add(streamContent, "file", fileName);
        
        var response = await _httpClient.PostAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Attachments}", content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaAttachment>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "uploading attachment");
        return result.Data ?? throw new AsanaException("Attachment data was null");
    }

    public async Task<AsanaAttachment> AddAttachmentToTaskAsync(string taskId, string attachmentUrl, CancellationToken cancellationToken = default)
    {
        var request = new { data = new { url = attachmentUrl } };
        var response = await _httpClient.PostAsJsonAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Attachments}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaAttachment>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "adding attachment");
        return result.Data ?? throw new AsanaException("Attachment data was null");
    }

    public async Task AddLikeToTaskAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.AddLike}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveLikeFromTaskAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.RemoveLike}", null, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AsanaTask> MoveTaskToSectionAsync(string taskId, string sectionId, string insertBefore = null, string insertAfter = null, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            data = new
            {
                task = taskId,
                insert_before = insertBefore,
                insert_after = insertAfter
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"{AsanaConstants.Endpoints.Sections}/{sectionId}/{AsanaConstants.Endpoints.AddTask}", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaTask>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "moving task");
        return result.Data ?? throw new AsanaException("Task data was null");
    }

    public async Task<AsanaTask> DuplicateTaskAsync(string taskId, string name = null, bool includeSubtasks = false, bool includeDependencies = false, CancellationToken cancellationToken = default)
    {
        var request = new
        {
            data = new
            {
                name = name,
                include_subtasks = includeSubtasks,
                include_dependencies = includeDependencies
            }
        };

        var response = await _httpClient.PostAsJsonAsync($"{AsanaConstants.Endpoints.Tasks}/{taskId}/{AsanaConstants.Endpoints.Duplicate}", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AsanaResponse<AsanaTask>>(cancellationToken: cancellationToken);
        if (result == null)
            throw new AsanaException("Failed to deserialize response");
        
        ThrowIfError(result, "duplicating task");
        return result.Data ?? throw new AsanaException("Task data was null");
    }

    private void ErrorCallback(Exception ex)
    {
        _errorCallback?.Invoke(ex);
    }

    private void ThrowIfError<T>(AsanaResponse<T> response, string operation)
    {
        if (response.Errors?.Any() == true)
        {
            var error = response.Errors.First();
            throw new AsanaException($"Error during {operation}: {error.Message}");
        }
    }
}
