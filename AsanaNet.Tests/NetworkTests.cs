using System;
using System.Collections.Generic;
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
    public class NetworkTests : TestBase
    {
        [Fact]
        public async Task ApiCall_WhenCancelled_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            
            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>(async (request, token) =>
                {
                    await Task.Delay(1000, token); // Simulate network delay
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync(cts.Token);
            cts.Cancel();
            await act.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task ApiCall_WhenNetworkTimeout_ShouldThrowHttpRequestException()
        {
            // Arrange
            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Connection timed out"));

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync();
            await act.Should().ThrowAsync<HttpRequestException>()
                .WithMessage("Connection timed out");
        }

        [Fact]
        public async Task ApiCall_WhenMalformedJsonResponse_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ invalid json }")
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync();
            await act.Should().ThrowAsync<AsanaException>()
                .WithMessage("Failed to deserialize response");
        }

        [Fact]
        public async Task ApiCall_WhenEmptyResponse_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync();
            await act.Should().ThrowAsync<AsanaException>()
                .WithMessage("Failed to deserialize response");
        }

        [Fact]
        public async Task ApiCall_WhenResponseWithoutData_ShouldThrowAsanaException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ \"not_data\": null }")
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            Func<Task> act = async () => await Client.GetMeAsync();
            await act.Should().ThrowAsync<AsanaException>()
                .WithMessage("Failed to deserialize response");
        }
    }
} 