using System;
using System.Net;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Text.Json;
using System.Threading;

namespace AsanaNet.Tests
{
    public class ErrorHandlingTests : TestBase
    {
        [Fact]
        public async Task GetMeAsync_WhenUnauthorized_ShouldThrowAsanaException()
        {
            // Arrange
            SetupMockErrorResponse("/users/me", HttpStatusCode.Unauthorized, "Not Authorized");

            // Act & Assert
            await Client.Invoking(c => c.GetMeAsync(CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Not Authorized");
        }

        [Fact]
        public async Task CreateTaskAsync_WhenBadRequest_ShouldThrowAsanaException()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest();
            SetupMockErrorResponse("/tasks", HttpStatusCode.BadRequest, "Invalid request parameters");

            // Act & Assert
            await Client.Invoking(c => c.CreateTaskAsync(request, CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Invalid request parameters");
        }

        [Fact]
        public async Task GetWorkspacesAsync_WhenInternalServerError_ShouldThrowAsanaException()
        {
            // Arrange
            SetupMockErrorResponse("/workspaces", HttpStatusCode.InternalServerError, "Internal server error");

            // Act & Assert
            await Client.Invoking(c => c.GetWorkspacesAsync(CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Internal server error");
        }

        [Fact]
        public async Task GetWorkspacesAsync_WhenEmptyErrorList_ShouldThrowAsanaException()
        {
            // Arrange
            SetupMockErrorResponse("/workspaces", HttpStatusCode.BadRequest, "Bad Request");

            // Act & Assert
            await Client.Invoking(c => c.GetWorkspacesAsync(CancellationToken.None))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Bad Request");
        }
    }
} 