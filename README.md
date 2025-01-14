# AsanaNet v2

## 🚨 This project has been modernized and updated for .NET 9 🚨

### About
This project is a modernized implementation of **AsanaNet**, a .NET library for interacting with the Asana REST API. Originally created by **Antony Woods** in 2012, this updated version has been refactored by **Nick Cassidy** in 2025 to target **.NET 9** and to use a more modern development approach.

This open-source project uses the MIT License. See the [LICENSE](./LICENSE) file for details.

---

## Latest Updates in v2.1.0-alpha.1
- **Comprehensive Sample Project**: Complete working example demonstrating all core features
- **Improved Error Handling**: Enhanced error handling with detailed console output and Data dictionary support
- **Authentication Examples**: Demonstrations of both Basic and OAuth authentication flows
- **File Operations**: Robust file handling examples with proper cleanup
- **Parallel Processing**: Examples of concurrent task management
- **Package Updates**: All dependencies updated to stable .NET 9.0.0 versions
- **Documentation**: Enhanced setup instructions and API usage examples

## Core Features from v2.1.0-alpha
- **Enhanced Test Coverage**: Comprehensive test suite with 87% line coverage
- **Improved Error Handling**: Robust error handling with detailed exception messages and event-based notifications
- **Request Validation**: Strong validation for all API operations with descriptive error messages
- **Mock Infrastructure**: Enhanced testing infrastructure for reliable unit tests with support for HTTP mocking
- **Documentation**: Detailed test reports and API documentation with usage examples
- **Authentication**: Support for both Basic (API Key) and OAuth authentication methods
- **File Handling**: Improved multipart form data handling for file uploads
- **Type Safety**: Enhanced null handling and validation throughout the codebase

## Core Features
- **Target Framework**: .NET 9.0 with full async support
- **Modern C# Features**: 
  - Required properties
  - Nullable reference types
  - Record types for immutable models
  - Pattern matching
  - Enhanced type inference
- **Dependency Injection**: 
  - Full support for Microsoft.Extensions.DependencyInjection 9.0.0
  - Scoped and transient service registration
  - Configuration binding support
- **Async/Await**: 
  - Fully asynchronous API implementation
  - Cancellation token support
  - Proper exception propagation
  - Task-based operations
- **Modern JSON Handling**: 
  - Uses System.Text.Json for better performance
  - Custom converters for Asana types
  - Proper datetime handling
  - Enum support
- **Error Handling**: 
  - Exception-based error handling
  - Event-based error notifications
  - Detailed error messages with Data dictionary support
  - Custom exception types
  - Comprehensive error context
- **Type Safety**: 
  - Strong typing throughout
  - Proper null handling
  - Validation attributes
  - Guard clauses
- **Authentication**: 
  - Basic authentication with API key
  - OAuth token support
  - Secure header handling
  - Token validation
- **File Operations**: 
  - Robust file upload handling
  - Proper content type management
  - Stream handling
  - Multipart form data support
- **Test Coverage**: 
  - Unit tests
  - Integration tests
  - Mock support
  - High coverage metrics

---

## Installation

### NuGet Package Manager Console
```powershell
Install-Package AsanaNet -Version 2.1.0-alpha
```

### .NET CLI
```powershell
dotnet add package AsanaNet --version 2.1.0-alpha
```

### Package Reference
```xml
<PackageReference Include="AsanaNet" Version="2.1.0-alpha" />
```

## Using AsanaNet

### Basic Setup
You can set up the Asana client in two ways:

1. Direct instantiation:
```csharp
// Basic authentication with API key
var asanaBasic = new Asana(YOUR_API_KEY, AuthenticationType.Basic);

// OAuth authentication
var asanaOAuth = new Asana(YOUR_OAUTH_TOKEN, AuthenticationType.OAuth);

// With custom HttpClient
var httpClient = new HttpClient() { BaseAddress = new Uri("https://app.asana.com/api/1.0/") };
var asanaCustom = new Asana(YOUR_API_KEY, AuthenticationType.Basic, httpClient: httpClient);
```

2. Using dependency injection:
```csharp
// In Startup.cs or Program.cs
services.AddAsanaNet(options => {
    options.ApiKey = YOUR_API_KEY;
    options.AuthenticationType = AuthenticationType.Basic;
    // Optional: Configure HttpClient
    options.ConfigureHttpClient = client => {
        client.Timeout = TimeSpan.FromSeconds(30);
    };
});

// In your service class
public class MyService
{
    private readonly IAsanaClient _asana;

    public MyService(IAsanaClient asana)
    {
        _asana = asana;
    }
}
```

### Core Operations

#### Fetching the Current User
```csharp
// Basic user fetch
var me = await asana.GetMeAsync();
Console.WriteLine($"Hello, {me.Name}");

// With cancellation token
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
var user = await asana.GetMeAsync(cts.Token);
```

#### Working with Workspaces
```csharp
// Get all workspaces
var workspaces = await asana.GetWorkspacesAsync();
foreach (var workspace in workspaces)
{
    Console.WriteLine($"Workspace: {workspace.Name} (ID: {workspace.Id})");
}

// Get teams in a workspace
var teams = await asana.GetTeamsInWorkspaceAsync(workspace.Id);
```

#### Managing Teams
```csharp
// Get teams in workspace
var teams = await asana.GetTeamsInWorkspaceAsync(workspace.Id);
foreach (var team in teams)
{
    Console.WriteLine($"Team: {team.Name} (ID: {team.Id})");
    
    // Get team members
    var members = await asana.GetTeamMembersAsync(team.Id);
    foreach (var member in members)
    {
        Console.WriteLine($"Member: {member.Name}");
    }
}
```

#### Task Operations
```csharp
// Create a task
var taskRequest = new AsanaTaskCreateRequest
{
    Name = "New Task",
    WorkspaceId = "123",
    Notes = "Task details",
    DueDate = DateTime.UtcNow.AddDays(7),
    AssigneeId = "456"
};
var task = await asana.CreateTaskAsync(taskRequest);

// Update a task
var updateRequest = new AsanaTaskUpdateRequest
{
    Name = "Updated Task Name",
    Notes = "Updated notes"
};
var updatedTask = await asana.UpdateTaskAsync(task.Id, updateRequest);

// Upload an attachment
using var fileStream = File.OpenRead("document.pdf");
var attachment = await asana.UploadAttachmentToTaskAsync(
    task.Id, 
    fileStream, 
    "document.pdf",
    "application/pdf"
);

// Delete a task
await asana.DeleteTaskAsync(task.Id);

// Manage dependencies
await asana.AddDependencyToTaskAsync(task.Id, dependentTaskId);
await asana.RemoveDependencyFromTaskAsync(task.Id, dependentTaskId);
```

### Error Handling
The client supports modern exception handling and event-based error notifications:

```csharp
// Event-based error handling
asana.OnError += (exception) => {
    Console.WriteLine($"API Error: {exception.Message}");
    // Standard error handling
    if (exception.Data.Count > 0)
    {
        Console.WriteLine("Additional error details:");
        foreach (var key in exception.Data.Keys)
        {
            Console.WriteLine($"{key}: {exception.Data[key]}");
        }
    }
    // Log or handle the error as needed
};

// Try-catch with specific exceptions
try
{
    var me = await asana.GetMeAsync();
}
catch (AsanaUnauthorizedException ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (AsanaNotFoundException ex)
{
    Console.WriteLine($"Resource not found: {ex.Message}");
}
catch (AsanaValidationException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
}
catch (AsanaException ex)
{
    Console.WriteLine($"Asana API Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

---

## Requirements
- **.NET 9.0** or higher
- **Visual Studio 2022** (v17.8 or higher) or **Visual Studio Code** with C# extensions
- **Windows**, **macOS**, or **Linux** operating system

## Dependencies
- Microsoft.Extensions.DependencyInjection (= 9.0.0)
- Microsoft.Extensions.Configuration.Json (= 9.0.0)
- System.Text.Json (= 9.0.0)
- Microsoft.Extensions.Http (= 9.0.0)

## Test Coverage
- **Lines**: 87% (4,523/5,200 lines)
- **Branches**: 82% (1,230/1,500 branches)
- **Methods**: 91% (342/376 methods)
- **Total Tests**: 54 (all passing)
- **Test Categories**:
  - Unit Tests: 42
  - Integration Tests: 12
- **Test Execution Time**: 1.0s

For detailed test information, see [2.1.0-alpha_TEST_REPORT.md](./2.1.0-alpha_TEST_REPORT.md).

---

## Sample Project
The `AsanaNet.Sample` project in the solution provides a comprehensive working example demonstrating:
- Basic and OAuth authentication with detailed examples
- Enhanced error handling with detailed console output
- File operations with proper cleanup and best practices
- Task management and dependencies
- Team and workspace operations
- Parallel processing examples
- Configuration management
- Error handling with Data dictionary support
- Complete authentication flows
- Resource cleanup patterns

## Documentation
- [Test Report](./2.1.0-alpha_TEST_REPORT.md) - Detailed testing documentation
- [CHANGELOG.md](./CHANGELOG.md) - Version history and changes
- [LICENSE](./LICENSE) - MIT License details
- [API Documentation](./docs/api.md) - API reference and examples
- [Contributing Guide](./CONTRIBUTING.md) - Guidelines for contributors

---

## Known Issues and Limitations
- OAuth token refresh functionality is not yet implemented
- Pagination support is in development
- Rate limiting handling needs improvement
- Some advanced API features are not yet supported:
  - Portfolios
  - Custom fields
  - Webhooks
  - Batch operations
  - Advanced search
- Performance optimizations pending for large dataset operations

See the test report for a complete list of known issues and planned improvements.

---

## Contributing
Contributions are welcome! Please follow these steps:

1. Check existing issues and the test report for known issues
2. Fork the repository
3. Create a feature branch (`git checkout -b feature/amazing-feature`)
4. Make your changes
5. Ensure all tests pass locally (`dotnet test`)
6. Add tests for new features
7. Update documentation
8. Commit your changes (`git commit -m 'Add amazing feature'`)
9. Push to the branch (`git push origin feature/amazing-feature`)
10. Open a Pull Request

### Code Style Guidelines
- Follow existing code patterns
- Use C# naming conventions
- Add XML documentation comments
- Include unit tests
- Keep methods focused and small
- Use async/await properly

---

## Support
- Create an issue for bug reports
- Use discussions for questions
- Check documentation first
- Search existing issues before creating new ones

## Security
- Report security issues privately
- Keep dependencies updated
- Use latest stable releases
- Follow security best practices

---

## Acknowledgments
- **Original Author**: Antony Woods (2012)
- **Modernization and Updates**: Nick Cassidy (2025)
- **Contributors**: See [CONTRIBUTORS.md](./CONTRIBUTORS.md)

---

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

---

## Project Status
![Build Status](https://github.com/GrouchyGaucho/AsanaNet/workflows/CI/badge.svg)
![Test Coverage](https://img.shields.io/badge/coverage-87%25-brightgreen.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)
