using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AsanaNet;
using AsanaNet.Models;
using AsanaNet.Enums;
using AsanaNet.Options;
using AsanaNet.Exceptions;

namespace AsanaNet.Sample;

class Program
{
    private static readonly CancellationTokenSource _cts = new();
    private static IAsanaClient _basicAuthClient = null!;
    private static IAsanaClient _oAuthClient = null!;
    private static HttpClient _httpClient = null!;
    private const string AuthorizationEndpoint = "https://app.asana.com/-/oauth_authorize";
    private const string TokenEndpoint = "https://app.asana.com/-/oauth_token";
    private const string RevokeEndpoint = "https://app.asana.com/-/oauth_revoke";

    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting AsanaNet Sample...");
            
            Console.CancelKeyPress += (s, e) => {
                e.Cancel = true;
                _cts.Cancel();
                Console.WriteLine("\nCancellation requested. Cleaning up...");
            };

            Console.WriteLine("Initializing clients...");
            await InitializeClients();
            Console.WriteLine("Clients initialized successfully.");
            
            // Demonstrate OAuth flow if requested
            if (args.Length > 0 && args[0] == "--oauth")
            {
                await DemonstrateOAuthFlow();
                return;
            }

            Console.WriteLine("\nStarting sample operations...");
            await RunSamples();
        }
        catch (AsanaException ex)
        {
            Console.WriteLine($"\nAsana API Error: {ex.Message}");
            if (ex.Data.Count > 0)
            {
                Console.WriteLine("Additional error details:");
                foreach (var key in ex.Data.Keys)
                {
                    Console.WriteLine($"{key}: {ex.Data[key]}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled by user.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nUnexpected Error: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        finally
        {
            _cts.Dispose();
            Console.WriteLine("\nSample completed. Press any key to exit...");
            Console.ReadKey();
        }
    }

    private static async Task InitializeClients()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _httpClient = new HttpClient();

        var apiToken = config["ApiToken"] ?? throw new InvalidOperationException("API token not found in appsettings.json");

        // Register Basic Auth client
        services.AddScoped<IAsanaClient>(_ => new Asana(
            apiToken,
            AuthenticationType.Basic
        ));

        // Register OAuth client - use the same token for now since we're not doing OAuth flow
        services.AddScoped<IAsanaClient>(_ => new Asana(
            apiToken,
            AuthenticationType.Basic // Changed to Basic auth since we're using PAT
        ));

        var serviceProvider = services.BuildServiceProvider();
        _basicAuthClient = serviceProvider.GetRequiredService<IAsanaClient>();
        _oAuthClient = serviceProvider.GetRequiredService<IAsanaClient>();

        // Subscribe to error events
        _basicAuthClient.OnError += OnError;
        _oAuthClient.OnError += OnError;
    }

    private static async Task DemonstrateOAuthFlow()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var clientId = config["OAuth:ClientId"];
        var clientSecret = config["OAuth:ClientSecret"];
        var redirectUri = config["OAuth:RedirectUri"];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(redirectUri))
        {
            throw new InvalidOperationException("OAuth configuration missing in appsettings.json");
        }

        Console.WriteLine("\n## OAuth Flow Demonstration ##");

        // 1. Generate PKCE values
        var codeVerifier = GenerateCodeVerifier();
        var codeChallenge = GenerateCodeChallenge(codeVerifier);
        var state = GenerateState();

        // 2. Build authorization URL
        var authUrl = BuildAuthorizationUrl(clientId, redirectUri, state, codeChallenge);
        Console.WriteLine($"\nAuthorization URL (open in browser):\n{authUrl}");

        // 3. Wait for authorization code
        Console.Write("\nEnter the authorization code from the redirect URL: ");
        var authorizationCode = Console.ReadLine();

        if (string.IsNullOrEmpty(authorizationCode))
        {
            throw new InvalidOperationException("Authorization code is required");
        }

        // 4. Exchange code for tokens
        var tokens = await ExchangeCodeForTokens(clientId, clientSecret, redirectUri, authorizationCode, codeVerifier);
        Console.WriteLine("\nReceived tokens:");
        Console.WriteLine($"Access Token: {tokens.AccessToken}");
        Console.WriteLine($"Refresh Token: {tokens.RefreshToken}");
        Console.WriteLine($"Expires In: {tokens.ExpiresIn} seconds");

        // 5. Demonstrate token refresh
        Console.WriteLine("\nDemonstrating token refresh...");
        var newTokens = await RefreshAccessToken(clientId, clientSecret, tokens.RefreshToken);
        Console.WriteLine("Received new tokens:");
        Console.WriteLine($"New Access Token: {newTokens.AccessToken}");
        Console.WriteLine($"Expires In: {newTokens.ExpiresIn} seconds");

        // 6. Demonstrate token revocation
        Console.WriteLine("\nDemonstrating token revocation...");
        await RevokeToken(clientId, clientSecret, tokens.RefreshToken);
        Console.WriteLine("Token revoked successfully");
    }

    private static string GenerateCodeVerifier()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        return Convert.ToBase64String(challengeBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string GenerateState()
    {
        var bytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToHexString(bytes).ToLower();
    }

    private static string BuildAuthorizationUrl(string clientId, string redirectUri, string state, string codeChallenge)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["response_type"] = "code",
            ["state"] = state,
            ["code_challenge_method"] = "S256",
            ["code_challenge"] = codeChallenge,
            ["scope"] = "default"
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{AuthorizationEndpoint}?{queryString}";
    }

    private static async Task<TokenResponse> ExchangeCodeForTokens(
        string clientId, 
        string clientSecret, 
        string redirectUri, 
        string code, 
        string codeVerifier)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri,
            ["code"] = code,
            ["code_verifier"] = codeVerifier
        });

        var response = await _httpClient.PostAsync(TokenEndpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Token exchange failed: {responseContent}");
        }

        return JsonSerializer.Deserialize<TokenResponse>(responseContent) 
            ?? throw new Exception("Failed to deserialize token response");
    }

    private static async Task<TokenResponse> RefreshAccessToken(string clientId, string clientSecret, string refreshToken)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["refresh_token"] = refreshToken
        });

        var response = await _httpClient.PostAsync(TokenEndpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Token refresh failed: {responseContent}");
        }

        return JsonSerializer.Deserialize<TokenResponse>(responseContent)
            ?? throw new Exception("Failed to deserialize token response");
    }

    private static async Task RevokeToken(string clientId, string clientSecret, string token)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["token"] = token
        });

        var response = await _httpClient.PostAsync(RevokeEndpoint, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Token revocation failed: {responseContent}");
        }
    }

    private static async Task RunSamples()
    {
        Console.WriteLine("# AsanaNet v2.1.0-alpha Comprehensive Sample #");
        var startTime = DateTime.Now;

        // 1. Authentication Comparison
        await DemonstrateAuthentication();

        // 2. User & Workspace Operations
        var (me, workspace) = await DemonstrateUserAndWorkspaceOperations();
        if (workspace == null) return;

        // 3. Team & Project Management
        await DemonstrateTeamAndProjectManagement(workspace);

        // 4. Task Operations & Dependencies
        await DemonstrateTaskOperations(workspace);

        // 5. File Operations
        await DemonstrateFileOperations(workspace.Id);

        // 6. Error Handling Scenarios
        await DemonstrateErrorHandling();

        // 7. Parallel Operations
        await DemonstrateParallelOperations(workspace);

        Console.WriteLine($"\nTotal execution time: {DateTime.Now - startTime}");
    }

    private static async Task DemonstrateAuthentication()
    {
        Console.WriteLine("\n## Authentication Comparison ##");
        
        var basicAuthUser = await _basicAuthClient.GetMeAsync(_cts.Token);
        Console.WriteLine($"Basic Auth User: {basicAuthUser?.Name}");

        var oAuthUser = await _oAuthClient.GetMeAsync(_cts.Token);
        Console.WriteLine($"OAuth User: {oAuthUser?.Name}");
    }

    private static async Task<(AsanaUser? User, AsanaWorkspace? Workspace)> DemonstrateUserAndWorkspaceOperations()
    {
        Console.WriteLine("\n## User & Workspace Operations ##");
        
        var me = await _basicAuthClient.GetMeAsync(_cts.Token);
        Console.WriteLine($"Current User: {me?.Name} (Email: {me?.Email})");

        var workspaces = await _basicAuthClient.GetWorkspacesAsync(_cts.Token);
        foreach (var workspace in workspaces)
        {
            Console.WriteLine($"Workspace: {workspace.Name} (ID: {workspace.Id})");
        }

        return (me, workspaces.FirstOrDefault());
    }

    private static async Task DemonstrateTeamAndProjectManagement(AsanaWorkspace workspace)
    {
        Console.WriteLine("\n## Team & Project Management ##");

        var teams = await _basicAuthClient.GetTeamsInWorkspaceAsync(workspace, _cts.Token);
        foreach (var team in teams)
        {
            Console.WriteLine($"Team: {team.Name} (ID: {team.Id})");
        }
    }

    private static async Task DemonstrateTaskOperations(AsanaWorkspace workspace)
    {
        Console.WriteLine("\n## Task Operations ##");

        // Create a task
        var task = await CreateTask(workspace.Id, "Test Task", "This is a test task");
        Console.WriteLine($"Created task: {task.Name} (ID: {task.Id})");

        // Update task
        var updateRequest = new AsanaTaskUpdateRequest
        {
            Name = "Updated Task Name",
            Notes = "Updated task notes"
        };

        var updatedTask = await _basicAuthClient.UpdateTaskAsync(task.Id, updateRequest, _cts.Token);
        Console.WriteLine($"Updated task: {updatedTask.Name}");

        // Cleanup
        await _basicAuthClient.DeleteTaskAsync(task.Id, _cts.Token);
        Console.WriteLine("Deleted task");
    }

    private static async Task DemonstrateFileOperations(string workspaceId)
    {
        Console.WriteLine("\n## File Operations ##");

        // Create a task for attachments
        var task = await CreateTask(workspaceId, "File Test Task", "This task will have attachments");
        Console.WriteLine($"Created task: {task.Name} (ID: {task.Id})");

        // Upload multiple file types
        var fileTypes = new[] { ("text.txt", "Text content"), ("data.json", "{ \"test\": true }") };
        
        foreach (var (filename, content) in fileTypes)
        {
            File.WriteAllText(filename, content);
            using (var fileStream = File.OpenRead(filename))
            {
                var attachment = await _basicAuthClient.UploadAttachmentToTaskAsync(
                    task.Id, fileStream, filename, "application/octet-stream", _cts.Token);
                Console.WriteLine($"Uploaded {filename}: {attachment.Name}");
            }
            File.Delete(filename);
        }

        // Cleanup
        await _basicAuthClient.DeleteTaskAsync(task.Id, _cts.Token);
        Console.WriteLine("Deleted task");
    }

    private static async Task DemonstrateErrorHandling()
    {
        Console.WriteLine("\n## Error Handling Scenarios ##");

        try
        {
            // Test invalid workspace
            var workspace = new AsanaWorkspace { Id = "invalid-id" };
            await _basicAuthClient.GetTeamsInWorkspaceAsync(workspace, _cts.Token);
        }
        catch (AsanaException ex)
        {
            Console.WriteLine($"Expected error (invalid workspace): {ex.Message}");
        }

        try
        {
            // Test rate limiting
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(_basicAuthClient.GetMeAsync(_cts.Token));
            }
            await Task.WhenAll(tasks);
        }
        catch (AsanaException ex)
        {
            Console.WriteLine($"Rate limit handling: {ex.Message}");
        }
    }

    private static async Task DemonstrateParallelOperations(AsanaWorkspace workspace)
    {
        Console.WriteLine("\n## Parallel Operations ##");

        // Create multiple tasks in parallel
        var taskCreationTasks = new List<Task<AsanaTask>>();
        for (int i = 1; i <= 3; i++)
        {
            var request = new AsanaTaskCreateRequest
            {
                Name = $"Parallel Task {i}",
                Notes = $"Created in parallel operation {i}",
                WorkspaceId = workspace.Id
            };
            taskCreationTasks.Add(_basicAuthClient.CreateTaskAsync(request, _cts.Token));
        }

        var tasks = await Task.WhenAll(taskCreationTasks);
        Console.WriteLine($"Created {tasks.Length} tasks in parallel");

        // Cleanup in parallel
        var deletionTasks = tasks.Select(t => 
            _basicAuthClient.DeleteTaskAsync(t.Id, _cts.Token));
        await Task.WhenAll(deletionTasks);
        Console.WriteLine("Cleaned up parallel tasks");
    }

    private static async Task<AsanaTask> CreateTask(string workspaceId, string name, string notes)
    {
        var request = new AsanaTaskCreateRequest
        {
            Name = name,
            Notes = notes,
            WorkspaceId = workspaceId
        };

        return await _basicAuthClient.CreateTaskAsync(request, _cts.Token);
    }

    private static void OnError(Exception ex)
    {
        Console.WriteLine($"Asana API Error: {ex.Message}");
        if (ex is AsanaException asanaEx)
        {
            Console.WriteLine($"Error Details: {asanaEx.Message}");
            // Rate limiting info is available in response headers
            Console.WriteLine("Check response headers for rate limiting information");
        }
    }
}

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = "";

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "";
}