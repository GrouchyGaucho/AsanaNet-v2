using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using System.Linq;

namespace AsanaNet
{
public class Asana : IAsanaClient
{
    private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly AuthenticationType _authType;
        private readonly string? _oAuthToken;

        public string ApiKey => _apiKey;
        public string BaseUrl { get; } = "https://app.asana.com/api/1.0/";
        public AuthenticationType AuthType => _authType;
        public string? OAuthToken => _oAuthToken;

        public event Action<Exception>? OnError;

        public Asana(string apiKey, AuthenticationType authType = AuthenticationType.Basic, string? oAuthToken = null, HttpClient? httpClient = null)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _authType = authType;
            _oAuthToken = oAuthToken;

            _httpClient = httpClient ?? new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        
            if (httpClient == null)
            {
                ConfigureAuthentication();
            }
        }

        private void ConfigureAuthentication()
        {
            switch (_authType)
            {
                case AuthenticationType.Basic:
                    var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:"));
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    break;
                case AuthenticationType.OAuth:
                    if (string.IsNullOrEmpty(_oAuthToken))
                        throw new ArgumentException("OAuth token is required when using OAuth authentication", nameof(_oAuthToken));
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _oAuthToken);
                    break;
                default:
                    throw new ArgumentException($"Unsupported authentication type: {_authType}");
            }
        }

        private async Task<T> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
                if (result == null)
                {
                    throw new AsanaException("Failed to deserialize response");
                }
                return result;
            }
            catch (JsonException ex)
            {
                throw new AsanaException("Failed to deserialize response", ex);
            }
        }

        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            try
            {
                var errorContent = await ReadFromJsonAsync<AsanaErrorResponse>(response);
                var errorMessage = errorContent?.Errors?.FirstOrDefault()?.Message ?? "Unknown error";
                var exception = new AsanaException($"API Error: {errorMessage}");
                OnError?.Invoke(exception);
                throw exception;
            }
            catch (AsanaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var exception = new AsanaException($"API Error: {response.StatusCode}", ex);
                OnError?.Invoke(exception);
                throw exception;
            }
        }

        public async Task<AsanaUser> GetMeAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("users/me", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaUser>>(response, cancellationToken);
            return result.Data;
        }

        public async Task<AsanaWorkspace[]> GetWorkspacesAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("workspaces", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaWorkspace[]>>(response, cancellationToken);
            return result.Data;
        }

        public async Task<AsanaTeam[]> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));
            if (string.IsNullOrEmpty(workspace.Id)) throw new ArgumentException("Workspace ID cannot be empty", nameof(workspace));

            var response = await _httpClient.GetAsync($"workspaces/{workspace.Id}/teams", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaTeam[]>>(response, cancellationToken);
            return result.Data;
    }

    public async Task<AsanaTask> CreateTaskAsync(AsanaTaskCreateRequest request, CancellationToken cancellationToken = default)
    {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(request.Name)) throw new ArgumentException("Task name cannot be empty", nameof(request));
            if (string.IsNullOrEmpty(request.WorkspaceId)) throw new ArgumentException("Workspace ID cannot be empty", nameof(request));

            var response = await _httpClient.PostAsJsonAsync("tasks", new { data = request }, cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaTask>>(response, cancellationToken);
            return result.Data;
    }

    public async Task<AsanaTask> UpdateTaskAsync(string taskId, AsanaTaskUpdateRequest request, CancellationToken cancellationToken = default)
    {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
            if (request == null) throw new ArgumentNullException(nameof(request));

            var response = await _httpClient.PutAsJsonAsync($"tasks/{taskId}", new { data = request }, cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaTask>>(response, cancellationToken);
            return result.Data;
    }

    public async Task DeleteTaskAsync(string taskId, CancellationToken cancellationToken = default)
    {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));

            var response = await _httpClient.DeleteAsync($"tasks/{taskId}", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
        }

        public async Task<AsanaTask[]> GetTaskDependenciesAsync(string taskId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));

            var response = await _httpClient.GetAsync($"tasks/{taskId}/dependencies", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaTask[]>>(response, cancellationToken);
            return result.Data;
    }

    public async Task AddTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default)
    {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
            if (string.IsNullOrEmpty(dependencyTaskId)) throw new ArgumentException("Dependency task ID cannot be empty", nameof(dependencyTaskId));

            var response = await _httpClient.PostAsJsonAsync($"tasks/{taskId}/addDependencies", new { data = new { dependency = dependencyTaskId } }, cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
        }

        public async Task<AsanaAttachment[]> GetTaskAttachmentsAsync(string taskId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));

            var response = await _httpClient.GetAsync($"tasks/{taskId}/attachments", cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaAttachment[]>>(response, cancellationToken);
            return result.Data;
    }

    public async Task<AsanaAttachment> UploadAttachmentToTaskAsync(string taskId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
            if (string.IsNullOrEmpty(taskId)) throw new ArgumentException("Task ID cannot be empty", nameof(taskId));
            if (fileStream == null) throw new ArgumentNullException(nameof(fileStream));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("File name cannot be empty", nameof(fileName));
            if (string.IsNullOrEmpty(contentType)) throw new ArgumentException("Content type cannot be empty", nameof(contentType));

        using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            content.Add(fileContent, "file", fileName);

            var response = await _httpClient.PostAsync($"tasks/{taskId}/attachments", content, cancellationToken);
            await EnsureSuccessStatusCodeAsync(response);
            var result = await ReadFromJsonAsync<AsanaResponse<AsanaAttachment>>(response, cancellationToken);
            return result.Data;
        }
    }
}
