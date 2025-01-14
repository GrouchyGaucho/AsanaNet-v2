using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;

namespace AsanaNet
{
    public interface IAsanaClient
    {
        string ApiKey { get; }
        string BaseUrl { get; }
        AuthenticationType AuthType { get; }
        string? OAuthToken { get; }
        event Action<Exception>? OnError;

        Task<AsanaUser> GetMeAsync(CancellationToken cancellationToken = default);
        Task<AsanaWorkspace[]> GetWorkspacesAsync(CancellationToken cancellationToken = default);
        Task<AsanaTeam[]> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default);
        
        Task<AsanaTask> CreateTaskAsync(AsanaTaskCreateRequest request, CancellationToken cancellationToken = default);
        Task<AsanaTask> UpdateTaskAsync(string taskId, AsanaTaskUpdateRequest request, CancellationToken cancellationToken = default);
        Task DeleteTaskAsync(string taskId, CancellationToken cancellationToken = default);
        
        Task<AsanaTask[]> GetTaskDependenciesAsync(string taskId, CancellationToken cancellationToken = default);
        Task AddTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default);
        
        Task<AsanaAttachment[]> GetTaskAttachmentsAsync(string taskId, CancellationToken cancellationToken = default);
        Task<AsanaAttachment> UploadAttachmentToTaskAsync(string taskId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    }
} 