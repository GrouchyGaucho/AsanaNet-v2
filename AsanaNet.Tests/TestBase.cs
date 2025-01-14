using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AsanaNet.Models;
using Moq;
using Moq.Protected;
using System.Text.Json;

namespace AsanaNet.Tests
{
    public abstract class TestBase : IDisposable
    {
        protected const string TestApiKey = "test_api_key";
        protected Mock<HttpMessageHandler> MockHttpHandler { get; }
        protected IAsanaClient Client { get; }
        private HttpRequestMessage? _lastRequest;

        protected TestBase()
        {
            MockHttpHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(MockHttpHandler.Object) { BaseAddress = new Uri("https://app.asana.com/api/1.0") };
            Client = new Asana(TestApiKey, AuthenticationType.Basic, httpClient: httpClient);
        }

        protected void SetupMockResponse<T>(string endpoint, AsanaResponse<T>? response = null, HttpMethod? method = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpResponse = new HttpResponseMessage(statusCode);
            if (response != null)
            {
                httpResponse.Content = JsonContent.Create(response);
            }

            MockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        (method == null || req.Method == method) &&
                        req.RequestUri!.AbsolutePath.EndsWith(endpoint)),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>(async (request, _) =>
                {
                    _lastRequest = request;
                    if (request.Content != null)
                    {
                        // Clone the content before it's disposed
                        var content = await request.Content.ReadAsStringAsync();
                        var contentType = request.Content.Headers.ContentType?.ToString();
                        if (request.Content is MultipartFormDataContent multipartContent)
                        {
                            _lastRequest.Content = new MultipartFormDataContent();
                            foreach (var part in multipartContent)
                            {
                                var partContent = new StreamContent(await part.ReadAsStreamAsync());
                                foreach (var header in part.Headers)
                                {
                                    partContent.Headers.Add(header.Key, header.Value);
                                }
                                ((MultipartFormDataContent)_lastRequest.Content).Add(partContent, part.Headers.ContentDisposition!.Name!.Trim('"'), part.Headers.ContentDisposition.FileName!.Trim('"'));
                            }
                        }
                        else
                        {
                            _lastRequest.Content = new StringContent(content);
                            if (contentType != null)
                            {
                                _lastRequest.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
                            }
                        }
                    }
                })
                .ReturnsAsync(httpResponse);
        }

        protected HttpRequestMessage? GetLastRequest() => _lastRequest;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
} 