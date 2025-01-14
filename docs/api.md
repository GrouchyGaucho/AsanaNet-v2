# AsanaNet API Documentation

## Table of Contents
- [Authentication](#authentication)
- [Client Configuration](#client-configuration)
- [Core Operations](#core-operations)
- [Task Management](#task-management)
- [Error Handling](#error-handling)

## Authentication

### Basic Authentication
```csharp
var client = new Asana("your_api_key", AuthenticationType.Basic);
```

### OAuth Authentication
```csharp
var client = new Asana("your_oauth_token", AuthenticationType.OAuth, oAuthToken: "your_oauth_token");
```

## Client Configuration

### Properties
- `ApiKey`: The API key used for authentication
- `BaseUrl`: The base URL for the Asana API (default: "https://app.asana.com/api/1.0/")
- `AuthType`: The authentication type (Basic or OAuth)
- `OAuthToken`: The OAuth token (if using OAuth authentication)

### Events
- `OnError`: Event triggered when an API error occurs

## Core Operations

### Get Current User
Retrieves the currently authenticated user.

```csharp
public async Task<AsanaUser> GetMeAsync(CancellationToken cancellationToken = default)
```

#### Parameters
- `cancellationToken`: Optional cancellation token

#### Returns
- `AsanaUser`: The current user's information

### Get Workspaces
Retrieves all workspaces accessible to the authenticated user.

```csharp
public async Task<AsanaWorkspace[]> GetWorkspacesAsync(CancellationToken cancellationToken = default)
```

#### Parameters
- `cancellationToken`: Optional cancellation token

#### Returns
- `AsanaWorkspace[]`: Array of accessible workspaces

### Get Teams in Workspace
Retrieves all teams in a specified workspace.

```csharp
public async Task<AsanaTeam[]> GetTeamsInWorkspaceAsync(AsanaWorkspace workspace, CancellationToken cancellationToken = default)
```

#### Parameters
- `workspace`: The workspace to get teams from
- `cancellationToken`: Optional cancellation token

#### Returns
- `AsanaTeam[]`: Array of teams in the workspace

## Task Management

### Create Task
Creates a new task.

```csharp
public async Task<AsanaTask> CreateTaskAsync(AsanaTaskCreateRequest request, CancellationToken cancellationToken = default)
```

#### Parameters
- `request`: Task creation request containing:
  - `Name` (required): Task name
  - `WorkspaceId` (required): ID of the workspace
  - `Notes`: Optional task notes
  - `DueDate`: Optional due date
  - `AssigneeId`: Optional assignee ID
- `cancellationToken`: Optional cancellation token

#### Returns
- `AsanaTask`: The created task

### Update Task
Updates an existing task.

```csharp
public async Task<AsanaTask> UpdateTaskAsync(string taskId, AsanaTaskUpdateRequest request, CancellationToken cancellationToken = default)
```

#### Parameters
- `taskId`: ID of the task to update
- `request`: Task update request containing modified properties
- `cancellationToken`: Optional cancellation token

#### Returns
- `AsanaTask`: The updated task

### Delete Task
Deletes a task.

```csharp
public async Task DeleteTaskAsync(string taskId, CancellationToken cancellationToken = default)
```

#### Parameters
- `taskId`: ID of the task to delete
- `cancellationToken`: Optional cancellation token

### Task Dependencies

#### Get Dependencies
```csharp
public async Task<AsanaTask[]> GetTaskDependenciesAsync(string taskId, CancellationToken cancellationToken = default)
```

#### Add Dependency
```csharp
public async Task AddTaskDependencyAsync(string taskId, string dependencyTaskId, CancellationToken cancellationToken = default)
```

### Task Attachments

#### Get Attachments
```csharp
public async Task<AsanaAttachment[]> GetTaskAttachmentsAsync(string taskId, CancellationToken cancellationToken = default)
```

#### Upload Attachment
```csharp
public async Task<AsanaAttachment> UploadAttachmentToTaskAsync(string taskId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
```

## Error Handling

### Exception Types
- `AsanaException`: Base exception for all Asana-related errors
- `AsanaUnauthorizedException`: Authentication or authorization failure
- `AsanaNotFoundException`: Requested resource not found
- `AsanaValidationException`: Invalid request parameters

### Event-Based Error Handling
```csharp
client.OnError += (exception) => {
    // Handle or log the error
};
```

### Response Error Handling
The client automatically handles HTTP status codes and deserializes error responses from the API. Error responses include:
- Error message
- Error details
- HTTP status code

## Models

### AsanaUser
```csharp
public class AsanaUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### AsanaWorkspace
```csharp
public class AsanaWorkspace
{
    public string Id { get; set; }
    public string Name { get; set; }
}
```

### AsanaTeam
```csharp
public class AsanaTeam
{
    public string Id { get; set; }
    public string Name { get; set; }
}
```

### AsanaTask
```csharp
public class AsanaTask
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public DateTime? DueDate { get; set; }
    public string AssigneeId { get; set; }
}
```

### AsanaAttachment
```csharp
public class AsanaAttachment
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string DownloadUrl { get; set; }
}
```

## Best Practices

1. **Error Handling**
   - Always handle potential exceptions
   - Use specific catch blocks for different error types
   - Consider implementing the OnError event handler

2. **Resource Management**
   - Use `using` statements for disposable resources
   - Properly handle stream disposal in file operations
   - Implement proper cancellation token handling

3. **Authentication**
   - Store API keys securely
   - Use environment variables or secure configuration
   - Implement proper token refresh for OAuth

4. **Performance**
   - Reuse HttpClient instances
   - Implement proper cancellation
   - Handle rate limiting appropriately

5. **Testing**
   - Mock HTTP responses in tests
   - Test error scenarios
   - Validate request parameters

## Rate Limiting

The Asana API implements rate limiting. While this client doesn't currently handle rate limiting automatically, you should:
1. Monitor response headers for rate limit information
2. Implement exponential backoff when needed
3. Cache responses when appropriate

## Thread Safety

The client is thread-safe for concurrent operations. However:
- Don't share HttpClient instances across different authentication contexts
- Be careful with event handler registration in concurrent scenarios
- Consider using dependency injection for better lifecycle management 