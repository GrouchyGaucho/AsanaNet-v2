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
            var expectedUser = new AsanaUser
            {
                Id = "123",
                Name = "Test User",
                Email = "test@example.com"
            };

            SetupMockResponse("/users/me", new AsanaResponse<AsanaUser> { Data = expectedUser });

            // Act
            var result = await Client.GetMeAsync();

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedUser.Id);
            result.Name.Should().Be(expectedUser.Name);
            result.Email.Should().Be(expectedUser.Email);
        }

        [Fact]
        public async Task GetMeAsync_WhenUnauthorized_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse
                {
                    Errors = new List<AsanaError> { new AsanaError { Message = "Not Authorized" } }
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.AbsolutePath.EndsWith("/users/me")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync();
            await act.Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Not Authorized");
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

            SetupMockResponse("/workspaces", new AsanaResponse<AsanaWorkspace[]> { Data = expectedWorkspaces });

            // Act
            var result = await Client.GetWorkspacesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be("1");
            result[0].Name.Should().Be("Workspace 1");
            result[1].Id.Should().Be("2");
            result[1].Name.Should().Be("Workspace 2");
        }

        [Fact]
        public async Task GetWorkspacesAsync_WhenUnauthorized_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse
                {
                    Errors = new List<AsanaError> { new AsanaError { Message = "Not Authorized" } }
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(r => r.RequestUri!.AbsolutePath.EndsWith("/workspaces")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Func<Task> act = async () => await Client.GetWorkspacesAsync();
            await act.Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Not Authorized");
        }
    }
} 