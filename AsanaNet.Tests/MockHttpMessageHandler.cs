using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsanaNet.Tests
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _defaultHandler;
        private readonly List<(Func<HttpRequestMessage, bool> Predicate, HttpResponseMessage Response)> _handlers = new();

        public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> defaultHandler)
        {
            _defaultHandler = defaultHandler ?? throw new ArgumentNullException(nameof(defaultHandler));
        }

        public void SetupRequest(Func<HttpRequestMessage, bool> predicate, HttpResponseMessage response)
        {
            _handlers.Add((predicate, response));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var (predicate, response) in _handlers)
            {
                if (predicate(request))
                {
                    return Task.FromResult(response);
                }
            }

            return Task.FromResult(_defaultHandler(request));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var (_, response) in _handlers)
                {
                    response.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
} 