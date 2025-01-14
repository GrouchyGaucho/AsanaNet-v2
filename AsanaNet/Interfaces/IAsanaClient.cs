using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;

namespace AsanaNet.Interfaces;

/// <summary>
/// Represents a client for interacting with the Asana API.
/// </summary>
public interface IAsanaClient
{
    /// <summary>
    /// Gets the currently authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The authenticated user or null if not found.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaUser?> GetMeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all workspaces accessible to the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of accessible workspaces.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaObjectCollection<AsanaWorkspace>> GetWorkspacesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all teams in the specified workspace.
    /// </summary>
    /// <param name="workspace">The workspace to get teams from.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of teams in the workspace.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaObjectCollection<AsanaTeam>> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all projects in the specified team.
    /// </summary>
    /// <param name="team">The team to get projects from.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of projects in the team.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaObjectCollection<AsanaProject>> GetProjectsInTeamAsync(AsanaTeam team, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all tasks in the specified project.
    /// </summary>
    /// <param name="project">The project to get tasks from.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of tasks in the project.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaObjectCollection<AsanaTask>> GetTasksInAProjectAsync(AsanaProject project, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets events in a project since the specified sync token.
    /// </summary>
    /// <param name="projectId">The ID of the project.</param>
    /// <param name="lastToken">Optional sync token from previous request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Events that occurred in the project.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaEvents> GetEventsInAProjectAsync(long projectId, string? lastToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a task by its ID.
    /// </summary>
    /// <param name="taskId">The ID of the task to retrieve.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The requested task or null if not found.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaTask?> GetTaskByIdAsync(string taskId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new task in Asana.
    /// </summary>
    /// <param name="request">The task creation request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created task.</returns>
    /// <exception cref="AsanaException">Thrown when the API returns an error or the response is invalid.</exception>
    Task<AsanaTask> CreateTaskAsync(AsanaTaskCreateRequest request, CancellationToken cancellationToken = default);

    Task<AsanaTask> UpdateTaskAsync(string taskId, AsanaTaskUpdateRequest request, CancellationToken cancellationToken = default);
    Task DeleteTaskAsync(string taskId, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection<AsanaTask>> GetSubtasksAsync(string taskId, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection<AsanaStory>> GetTaskStoriesAsync(string taskId, CancellationToken cancellationToken = default);
    Task<AsanaStory> AddCommentToTaskAsync(string taskId, string text, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection<AsanaTaskDependency>> GetTaskDependenciesAsync(string taskId, CancellationToken cancellationToken = default);
    Task AddTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default);
    Task RemoveTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection<AsanaAttachment>> GetTaskAttachmentsAsync(string taskId, CancellationToken cancellationToken = default);
    Task<AsanaAttachment> UploadAttachmentToTaskAsync(string taskId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<AsanaAttachment> AddAttachmentToTaskAsync(string taskId, string attachmentUrl, CancellationToken cancellationToken = default);
    Task AddLikeToTaskAsync(string taskId, CancellationToken cancellationToken = default);
    Task RemoveLikeFromTaskAsync(string taskId, CancellationToken cancellationToken = default);
    Task<AsanaTask> MoveTaskToSectionAsync(string taskId, string sectionId, string insertBefore = null, string insertAfter = null, CancellationToken cancellationToken = default);
    Task<AsanaTask> DuplicateTaskAsync(string taskId, string name = null, bool includeSubtasks = false, bool includeDependencies = false, CancellationToken cancellationToken = default);
} 