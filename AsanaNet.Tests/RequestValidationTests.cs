using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace AsanaNet.Tests
{
    public class RequestValidationTests : TestBase
    {
        [Fact]
        public async Task CreateTaskAsync_WhenRequestBodyInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            var request = new AsanaTaskCreateRequest();

            // Act & Assert
            await Client.Invoking(c => c.CreateTaskAsync(request, CancellationToken.None))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Task name cannot be empty*");
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WhenFileStreamNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Client.Invoking(c => c.UploadAttachmentToTaskAsync("123", null!, "test.txt", "text/plain", CancellationToken.None))
                .Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'fileStream')");
        }

        [Fact]
        public async Task UploadAttachmentToTaskAsync_WhenValidRequest_ShouldSendMultipartFormData()
        {
            // Arrange
            var taskId = "123";
            var fileName = "test.txt";
            var fileContent = "Test content";
            var contentType = "text/plain";
            var expectedAttachment = new AsanaAttachment { Id = "1", Name = fileName };

            using var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            SetupMockResponse($"/tasks/{taskId}/attachments", expectedAttachment, HttpMethod.Post);

            // Act
            var result = await Client.UploadAttachmentToTaskAsync(taskId, fileStream, fileName, contentType, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedAttachment.Id);
            result.Name.Should().Be(expectedAttachment.Name);

            LastRequest.Should().NotBeNull();
            LastRequest!.Content.Should().BeOfType<MultipartFormDataContent>();
            var multipartContent = (MultipartFormDataContent)LastRequest.Content;

            // Verify content disposition headers
            var formContent = multipartContent.Should().ContainSingle().Subject;
            formContent.Headers.ContentDisposition.Should().NotBeNull();
            formContent.Headers.ContentDisposition!.Name.Should().Be("\"file\"");
            formContent.Headers.ContentDisposition!.FileName.Should().Be($"\"{fileName}\"");
            formContent.Headers.ContentType!.MediaType.Should().Be(contentType);
        }
    }
} 