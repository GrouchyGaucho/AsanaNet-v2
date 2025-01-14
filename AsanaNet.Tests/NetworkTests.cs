using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AsanaNet.Tests
{
    public class NetworkTests : TestBase
    {
        [Fact]
        public async Task GetMeAsync_WhenNetworkError_ShouldThrowAsanaException()
        {
            // Arrange
            SetupMockErrorResponse("/users/me", HttpStatusCode.ServiceUnavailable, "Service Unavailable");

            // Act & Assert
            await Client.Invoking(c => c.GetMeAsync(CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Service Unavailable");
        }

        [Fact]
        public async Task GetWorkspacesAsync_WhenNetworkTimeout_ShouldThrowAsanaException()
        {
            // Arrange
            SetupMockErrorResponse("/workspaces", HttpStatusCode.GatewayTimeout, "Gateway Timeout");

            // Act & Assert
            await Client.Invoking(c => c.GetWorkspacesAsync(CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Gateway Timeout");
        }

        [Fact]
        public async Task GetTeamsInWorkspaceAsync_WhenInternalServerError_ShouldThrowAsanaException()
        {
            // Arrange
            var workspace = new AsanaWorkspace { Id = "123" };
            SetupMockErrorResponse($"/organizations/{workspace.Id}/teams", HttpStatusCode.InternalServerError, "Internal Server Error");

            // Act & Assert
            await Client.Invoking(c => c.GetTeamsInWorkspaceAsync(workspace, CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Internal Server Error");
        }

        [Fact]
        public async Task CreateTaskAsync_WhenBadGateway_ShouldThrowAsanaException()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest { Name = "Test Task", WorkspaceId = "123" };
            SetupMockErrorResponse("/tasks", HttpStatusCode.BadGateway, "Bad Gateway");

            // Act & Assert
            await Client.Invoking(c => c.CreateTaskAsync(request, CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Bad Gateway");
        }
    }
} 