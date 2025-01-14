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

namespace AsanaNet.Tests
{
    public class ErrorHandlingTests : TestBase
    {
        [Fact]
        public async Task ApiCall_WhenUnauthorized_ShouldThrowAsanaException()
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
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.AbsolutePath.EndsWith("/users/me")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            await Client.Invoking(c => c.GetMeAsync())
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Not Authorized");
        }

        [Fact]
        public async Task ApiCall_WhenNotFound_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse 
                { 
                    Errors = new List<AsanaError> { new AsanaError { Message = "Task not found" } }
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri!.AbsolutePath.EndsWith("/tasks/nonexistent")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            await Client.Invoking(c => c.DeleteTaskAsync("nonexistent"))
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Task not found");
        }

        [Fact]
        public async Task ApiCall_WhenBadRequest_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest { WorkspaceId = "123" }; // Missing required Name field

            // Act & Assert
            await FluentActions.Awaiting(() => Client.CreateTaskAsync(request))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Task name cannot be empty*");
        }

        [Fact]
        public async Task ApiCall_WhenServerError_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse 
                { 
                    Errors = new List<AsanaError> { new AsanaError { Message = "Internal server error" } }
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.AbsolutePath.EndsWith("/workspaces")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            await Client.Invoking(c => c.GetWorkspacesAsync())
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Internal server error");
        }

        [Fact]
        public async Task ApiCall_WhenErrorWithoutMessage_ShouldThrowAsanaExceptionWithUnknownError()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse 
                { 
                    Errors = new List<AsanaError>()
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.AbsolutePath.EndsWith("/workspaces")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            await Client.Invoking(c => c.GetWorkspacesAsync())
                .Should().ThrowAsync<AsanaException>()
                .WithMessage("API Error: Unknown error");
        }

        [Fact]
        public async Task ApiCall_WhenErrorOccurs_ShouldTriggerOnErrorEvent()
        {
            // Arrange
            Exception? caughtException = null;
            Client.OnError += (ex) => caughtException = ex;

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonSerializer.Serialize(new AsanaErrorResponse 
                { 
                    Errors = new List<AsanaError> { new AsanaError { Message = "Test error" } }
                }))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri!.AbsolutePath.EndsWith("/users/me")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            try
            {
                await Client.GetMeAsync();
            }
            catch
            {
                // Ignore the exception as we're testing the event
            }

            // Assert
            caughtException.Should().NotBeNull();
            caughtException.Should().BeOfType<AsanaException>();
            caughtException!.Message.Should().Be("API Error: Test error");
        }
    }
} 