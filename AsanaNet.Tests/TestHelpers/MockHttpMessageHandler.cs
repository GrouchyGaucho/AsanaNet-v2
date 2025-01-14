using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using Moq;
using Moq.Protected;

namespace AsanaNet.Tests.TestHelpers;

public static class MockHttpMessageHandlerSetup
{
    public static void SetupSuccessResponse<T>(
        Mock<HttpMessageHandler> mockHandler,
        string path,
        AsanaResponse<T> response,
        HttpMethod? method = null)
    {
        var json = JsonSerializer.Serialize(response);
        var content = new StringContent(json);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.PathAndQuery.Contains(path) &&
                    (method == null || req.Method == method)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            });
    }

    public static void SetupErrorResponse(
        Mock<HttpMessageHandler> mockHandler,
        string path,
        HttpStatusCode statusCode,
        string message,
        HttpMethod? method = null)
    {
        var error = new AsanaErrorResponse
        {
            Errors = new List<AsanaError> { new AsanaError { Message = message } }
        };

        var json = JsonSerializer.Serialize(error);
        var content = new StringContent(json);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.PathAndQuery.Contains(path) &&
                    (method == null || req.Method == method)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = content
            });
    }
} 