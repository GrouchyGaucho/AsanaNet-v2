using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using FluentAssertions;
using Xunit;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace AsanaNet.Tests
{
    public class AsanaClientTests : TestBase
    {
        [Fact]
        public async Task GetMeAsync_ShouldReturnUser()
        {
            // Arrange
            var expectedUser = new AsanaUser { Id = "123", Name = "Test User" };
            SetupMockResponse("/users/me", expectedUser);

            // Act
            var user = await Client.GetMeAsync(CancellationToken.None);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(expectedUser.Id);
            user.Name.Should().Be(expectedUser.Name);
        }

        [Fact]
        public async Task GetWorkspacesAsync_ShouldReturnWorkspaces()
        {
            // Arrange
            var expectedWorkspaces = new[]
            {
                new AsanaWorkspace { Id = "1", Name = "Workspace 1" },
                new AsanaWorkspace { Id = "2", Name = "Workspace 2" }
            };
            SetupMockResponse("/workspaces", expectedWorkspaces);

            // Act
            var workspaces = await Client.GetWorkspacesAsync(CancellationToken.None);

            // Assert
            workspaces.Should().NotBeNull();
            workspaces.Should().HaveCount(2);
            workspaces[0].Id.Should().Be(expectedWorkspaces[0].Id);
            workspaces[1].Name.Should().Be(expectedWorkspaces[1].Name);
        }

        [Fact]
        public async Task GetTeamsInWorkspaceAsync_ShouldReturnTeams()
        {
            // Arrange
            var workspace = new AsanaWorkspace { Id = "123" };
            var expectedTeams = new[]
            {
                new AsanaTeam { Id = "1", Name = "Team 1" },
                new AsanaTeam { Id = "2", Name = "Team 2" }
            };
            SetupMockResponse($"/organizations/{workspace.Id}/teams", expectedTeams);

            // Act
            var teams = await Client.GetTeamsInWorkspaceAsync(workspace, CancellationToken.None);

            // Assert
            teams.Should().NotBeNull();
            teams.Should().HaveCount(2);
            teams[0].Id.Should().Be(expectedTeams[0].Id);
            teams[1].Name.Should().Be(expectedTeams[1].Name);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnCreatedTask()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest
            {
                Name = "Test Task",
                WorkspaceId = "123",
                Notes = "Test Notes"
            };
            var expectedTask = new AsanaTask
            {
                Id = "1",
                Name = request.Name,
                Notes = request.Notes
            };
            SetupMockResponse("/tasks", expectedTask, HttpMethod.Post);

            // Act
            var task = await Client.CreateTaskAsync(request, CancellationToken.None);

            // Assert
            task.Should().NotBeNull();
            task.Id.Should().Be(expectedTask.Id);
            task.Name.Should().Be(expectedTask.Name);
            task.Notes.Should().Be(expectedTask.Notes);
        }
    }
} 