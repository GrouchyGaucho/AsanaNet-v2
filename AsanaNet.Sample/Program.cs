using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AsanaNet.Enums;
using AsanaNet.Models;

namespace AsanaNet.Sample;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var apiToken = GetApiToken();
            var asana = new Asana(apiToken, AuthenticationType.Basic, OnError);

            Console.WriteLine("# Asana .NET 9 Sample #");
            var startTime = DateTime.Now;

            // Get current user
            var me = await asana.GetMeAsync();
            Console.WriteLine($"Hello, {me?.Name}");

            // Get workspaces
            var workspaces = await asana.GetWorkspacesAsync();
            foreach (var workspace in workspaces.Data)
            {
                Console.WriteLine($"Workspace: {workspace.Name}");

                // Get teams in workspace
                var teams = await asana.GetTeamsInWorkspaceAsync(workspace);
                foreach (var team in teams.Data)
                {
                    Console.WriteLine($"  Team: {team.Name}");

                    // Get projects in team
                    var projects = await asana.GetProjectsInTeamAsync(team);
                    foreach (var project in projects.Data)
                    {
                        Console.WriteLine($"    Project: {project.Name}");

                        // Get tasks in project
                        var tasks = await asana.GetTasksInAProjectAsync(project);
                        foreach (var task in tasks.Data)
                        {
                            Console.WriteLine($"      Task: {task.Name}");
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Execution time: {DateTime.Now - startTime}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    private static void OnError(Exception ex)
    {
        Console.WriteLine($"Asana API Error: {ex.Message}");
    }

    private static string GetApiToken()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return config["ApiToken"] ?? throw new InvalidOperationException("API token not found in appsettings.json");
    }
}