using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;

namespace AsanaNet.Interfaces;

public interface IAsanaClient
{
    Task<AsanaUser?> GetMeAsync(CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection> GetWorkspacesAsync(CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection> GetProjectsInTeamAsync(AsanaTeam team, CancellationToken cancellationToken = default);
    Task<AsanaObjectCollection> GetTasksInAProjectAsync(AsanaProject project, CancellationToken cancellationToken = default);
    Task<AsanaEvents> GetEventsInAProjectAsync(long projectId, string? lastToken = null, CancellationToken cancellationToken = default);
} 