using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AsanaNet.Models;
using AsanaNet.Exceptions;
using FluentAssertions;
using Xunit;

namespace AsanaNet.Tests
{
    public class TaskOperationsTests : TestBase
    {
        [Fact]
        public async Task CreateTaskAsync_ShouldCreateTask()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest
            {
                Name = "Test Task",
                Notes = "Test Notes",
                WorkspaceId = "123"
            };

            var expectedTask = new AsanaTask
            {
                Id = "456",
                Name = request.Name,
                Notes = request.Notes
            };

            SetupMockResponse("/tasks", new AsanaResponse<AsanaTask> { Data = expectedTask }, HttpMethod.Post);

            // Act
            var result = await Client.CreateTaskAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedTask.Id);
            result.Name.Should().Be(expectedTask.Name);
            result.Notes.Should().Be(expectedTask.Notes);
        }

        [Fact]
        public async Task CreateTaskAsync_WithNullRequest_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Client.Invoking(c => c.CreateTaskAsync(null!))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            // Arrange
            var taskId = "456";
            var request = new AsanaTaskUpdateRequest
            {
                Name = "Updated Task",
                Notes = "Updated Notes"
            };

            var expectedTask = new AsanaTask
            {
                Id = taskId,
                Name = request.Name,
                Notes = request.Notes
            };

            SetupMockResponse($"/tasks/{taskId}", new AsanaResponse<AsanaTask> { Data = expectedTask }, HttpMethod.Put);

            // Act
            var result = await Client.UpdateTaskAsync(taskId, request);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.Name.Should().Be(request.Name);
            result.Notes.Should().Be(request.Notes);
        }

        [Fact]
        public async Task UpdateTaskAsync_WithNullRequest_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Client.Invoking(c => c.UpdateTaskAsync("123", null!))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateTaskAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.UpdateTaskAsync("", new AsanaTaskUpdateRequest()))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask()
        {
            // Arrange
            var taskId = "456";
            SetupMockResponse($"/tasks/{taskId}", new AsanaResponse<object> { Data = new { } }, HttpMethod.Delete);

            // Act & Assert
            await Client.Invoking(c => c.DeleteTaskAsync(taskId))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteTaskAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.DeleteTaskAsync(""))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetTaskDependenciesAsync_ShouldReturnDependencies()
        {
            // Arrange
            var taskId = "456";
            var expectedDependencies = new[]
            {
                new AsanaTask { Id = "1", Name = "Dependency 1" },
                new AsanaTask { Id = "2", Name = "Dependency 2" }
            };

            SetupMockResponse($"/tasks/{taskId}/dependencies", new AsanaResponse<AsanaTask[]> { Data = expectedDependencies });

            // Act
            var result = await Client.GetTaskDependenciesAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be("1");
            result[0].Name.Should().Be("Dependency 1");
            result[1].Id.Should().Be("2");
            result[1].Name.Should().Be("Dependency 2");
        }

        [Fact]
        public async Task GetTaskDependenciesAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.GetTaskDependenciesAsync(""))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddTaskDependencyAsync_ShouldAddDependency()
        {
            // Arrange
            var taskId = "456";
            var dependencyTaskId = "789";
            SetupMockResponse($"/tasks/{taskId}/addDependencies", new AsanaResponse<object> { Data = new { } }, HttpMethod.Post);

            // Act & Assert
            await Client.Invoking(c => c.AddTaskDependencyAsync(taskId, dependencyTaskId))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddTaskDependencyAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.AddTaskDependencyAsync("", "123"))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddTaskDependencyAsync_WithEmptyDependencyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.AddTaskDependencyAsync("123", ""))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetTaskAttachmentsAsync_ShouldReturnAttachments()
        {
            // Arrange
            var taskId = "456";
            var expectedAttachments = new[]
            {
                new AsanaAttachment { Id = "1", Name = "Attachment 1" },
                new AsanaAttachment { Id = "2", Name = "Attachment 2" }
            };

            SetupMockResponse($"/tasks/{taskId}/attachments", new AsanaResponse<AsanaAttachment[]> { Data = expectedAttachments });

            // Act
            var result = await Client.GetTaskAttachmentsAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be("1");
            result[0].Name.Should().Be("Attachment 1");
            result[1].Id.Should().Be("2");
            result[1].Name.Should().Be("Attachment 2");
        }

        [Fact]
        public async Task GetTaskAttachmentsAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Act & Assert
            await Client.Invoking(c => c.GetTaskAttachmentsAsync(""))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_ShouldUploadAttachment()
        {
            // Arrange
            var taskId = "456";
            var fileName = "test.txt";
            var contentType = "text/plain";
            var fileContent = "Test file content";
            var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

            var expectedAttachment = new AsanaAttachment
            {
                Id = "789",
                Name = fileName
            };

            SetupMockResponse($"/tasks/{taskId}/attachments", new AsanaResponse<AsanaAttachment> { Data = expectedAttachment }, HttpMethod.Post);

            // Act
            var result = await Client.UploadAttachmentToTaskAsync(taskId, fileStream, fileName, contentType);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedAttachment.Id);
            result.Name.Should().Be(expectedAttachment.Name);
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WithEmptyTaskId_ShouldThrowArgumentException()
        {
            // Arrange
            var fileStream = new MemoryStream(new byte[0]);

            // Act & Assert
            await Client.Invoking(c => c.UploadAttachmentToTaskAsync("", fileStream, "test.txt", "text/plain"))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WithNullFileStream_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Client.Invoking(c => c.UploadAttachmentToTaskAsync("123", null!, "test.txt", "text/plain"))
                .Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WithEmptyFileName_ShouldThrowArgumentException()
        {
            // Arrange
            var fileStream = new MemoryStream(new byte[0]);

            // Act & Assert
            await Client.Invoking(c => c.UploadAttachmentToTaskAsync("123", fileStream, "", "text/plain"))
                .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WithEmptyContentType_ShouldThrowArgumentException()
        {
            // Arrange
            var fileStream = new MemoryStream(new byte[0]);

            // Act & Assert
            await Client.Invoking(c => c.UploadAttachmentToTaskAsync("123", fileStream, "test.txt", ""))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
} 