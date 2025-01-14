using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using AsanaNet.Models;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace AsanaNet.Tests
{
    public abstract class TestBase : IDisposable
    {
        protected const string TestApiKey = "test_api_key";
        protected readonly IAsanaClient Client;
        private readonly HttpMessageHandler _mockHandler;
        private HttpRequestMessage? _lastRequest;
        protected HttpRequestMessage? LastRequest => _lastRequest;

        protected TestBase()
        {
            _mockHandler = new MockHttpMessageHandler(req =>
            {
                _lastRequest = req;
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = JsonContent.Create(new AsanaErrorResponse
                    {
                        Errors = new List<AsanaError>
                        {
                            new() { Message = "Not Found" }
                        }
                    })
                };
            });

            var httpClient = new HttpClient(_mockHandler) { BaseAddress = new Uri("https://app.asana.com/api/1.0") };
            Client = new Asana(TestApiKey, AuthenticationType.Basic, httpClient: httpClient);
        }

        protected void SetupMockResponse<T>(string endpoint, T? response = default, HttpMethod? method = null) where T : class
        {
            var mockHandler = (MockHttpMessageHandler)_mockHandler;
            mockHandler.SetupRequest(req =>
                req.RequestUri!.AbsolutePath.EndsWith(endpoint) &&
                (method == null || req.Method == method),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = response == null ? null : JsonContent.Create(new AsanaResponse<T> { Data = response })
                });
        }

        protected void SetupMockErrorResponse(string endpoint, HttpStatusCode statusCode, string errorMessage)
        {
            var mockHandler = (MockHttpMessageHandler)_mockHandler;
            mockHandler.SetupRequest(req =>
                req.RequestUri!.AbsolutePath.EndsWith(endpoint),
                new HttpResponseMessage(statusCode)
                {
                    Content = JsonContent.Create(new AsanaErrorResponse
                    {
                        Errors = new List<AsanaError>
                        {
                            new() { Message = errorMessage }
                        }
                    })
                });
        }

        public void Dispose()
        {
            _mockHandler.Dispose();
            GC.SuppressFinalize(this);
        }
    }
} 