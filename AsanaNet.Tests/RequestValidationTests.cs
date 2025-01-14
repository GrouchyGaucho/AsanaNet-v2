using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using FluentAssertions;
using FluentAssertions.Specialized;
using Xunit;
using Moq;
using Moq.Protected;
using System.Text.Json;

namespace AsanaNet.Tests
{
    public class RequestValidationTests : TestBase
    {
        [Fact]
        public async Task CreateTask_WhenRequestBodyInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidRequest = new AsanaTaskCreateRequest
            {
                Name = "",
                WorkspaceId = ""
            };

            // Act & Assert
            Func<Task> act = async () => await Client.CreateTaskAsync(invalidRequest);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Task name cannot be empty*");
        }

        [Fact]
        public async Task UploadAttachment_WhenFileStreamNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Stream? nullStream = null;

            // Act & Assert
            Func<Task> act = async () => await Client.UploadAttachmentToTaskAsync("123", nullStream!, "test.txt", "application/octet-stream");
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'fileStream')");
        }

        [Fact]
        public async Task UploadAttachment_WhenFileNameEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            // Act & Assert
            Func<Task> act = async () => await Client.UploadAttachmentToTaskAsync("123", stream, "", "application/octet-stream");
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("File name cannot be empty*");
        }

        [Fact]
        public async Task UploadAttachment_WhenTaskIdEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1, 2, 3 });

            // Act & Assert
            Func<Task> act = async () => await Client.UploadAttachmentToTaskAsync("", stream, "test.txt", "application/octet-stream");
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Task ID cannot be empty*");
        }

        [Fact]
        public async Task UploadAttachment_WhenValidRequest_ShouldSendMultipartFormData()
        {
            // Arrange
            var taskId = "123";
            var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });
            var fileName = "test.txt";
            var contentType = "text/plain";

            SetupMockResponse<AsanaAttachment>($"/tasks/{taskId}/attachments", new AsanaResponse<AsanaAttachment>
            {
                Data = new AsanaAttachment { Id = "456", Name = fileName }
            });

            // Act
            await Client.UploadAttachmentToTaskAsync(taskId, fileStream, fileName, contentType);

            // Assert
            var request = GetLastRequest();
            request.Should().NotBeNull();
            request!.RequestUri!.AbsolutePath.Should().EndWith($"/tasks/{taskId}/attachments");
            request.Content.Should().BeOfType<MultipartFormDataContent>();

            var multipartContent = (MultipartFormDataContent)request.Content;
            var fileContent = multipartContent.Should().ContainSingle().Subject;
            fileContent.Headers.ContentDisposition.Should().NotBeNull();
            fileContent.Headers.ContentDisposition!.Name.Should().Be("file");
            fileContent.Headers.ContentDisposition.FileName.Should().Be(fileName);
        }

        [Fact]
        public async Task GetTeamsInWorkspace_WhenWorkspaceIdEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var workspace = new AsanaWorkspace { Id = "" };

            // Act & Assert
            Func<Task> act = async () => await Client.GetTeamsInWorkspaceAsync(workspace);
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Workspace ID cannot be empty*");
        }
    }
} 