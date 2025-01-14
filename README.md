# AsanaNet (Modernized)

## 🚨 This project has been modernized and updated for .NET 9 🚨

### About
This project is a modernized implementation of **AsanaNet**, a .NET library for interacting with the Asana REST API. Originally created by **Antony Woods** in 2012, this updated version has been refactored by **Nick Cassidy** in 2025 to target **.NET 9** and to use a more modern development approach.

This open-source project uses the MIT License. See the [LICENSE](./LICENSE) file for details.

---

## Updates in the Modernized Version
- **Target Framework Updated**: The library now targets **.NET 9.0**
- **Modern C# Features**: Utilizes C# 11 features like required properties and improved nullable reference types
- **Dependency Injection**: Full support for Microsoft.Extensions.DependencyInjection
- **Async/Await**: Fully asynchronous API implementation with cancellation token support
- **Modern JSON Handling**: Uses System.Text.Json for better performance
- **Improved Error Handling**: Exception-based error handling with proper async/await support
- **Type Safety**: Strong typing throughout with proper null handling

---

## Installation
Install via NuGet Package Manager:
```powershell
dotnet add package AsanaNet
```

## Using AsanaNet

### Basic Setup
You can set up the Asana client in two ways:

1. Direct instantiation:
```csharp
var asana = new Asana(YOUR_API_TOKEN, AuthenticationType.OAuth);
```

2. Using dependency injection:
```csharp
services.AddAsanaNet(options => {
    options.ApiToken = YOUR_API_TOKEN;
});
```

### Fetching the Current User
```csharp
var me = await asana.GetMeAsync();
Console.WriteLine($"Hello, {me.Name}");
```

### Fetching Workspaces
```csharp
var workspaces = await asana.GetWorkspacesAsync();
foreach (var workspace in workspaces)
{
    Console.WriteLine($"Workspace: {workspace.Name}");
}
```

### Fetching Teams in a Workspace
```csharp
var teams = await asana.GetTeamsInWorkspaceAsync(workspace);
foreach (var team in teams)
{
    Console.WriteLine($"Team: {team.Name}");
}
```

### Fetching Projects in a Team
```csharp
var projects = await asana.GetProjectsInTeamAsync(team);
foreach (var project in projects)
{
    Console.WriteLine($"Project: {project.Name}");
}
```

### Error Handling
The client supports modern exception handling and event-based error notifications:

```csharp
asana.OnError += (exception) => {
    Console.WriteLine($"API Error: {exception.Message}");
};

try
{
    var me = await asana.GetMeAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

---

## Requirements
- **.NET 9.0** or higher
- **Visual Studio 2022** or **Visual Studio Code** with C# extensions

## Dependencies
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration.Json
- System.Text.Json

---

## Sample Project
Check out the `AsanaNet.Sample` project in the solution for a complete working example.

---

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

---

## Acknowledgments
- **Original Author**: Antony Woods (2012)
- **Modernization and Updates**: Nick Cassidy (2025)

---

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request. Make sure to read our contributing guidelines first.

For a detailed list of all changes, see the [CHANGELOG.md](./CHANGELOG.md) file.
