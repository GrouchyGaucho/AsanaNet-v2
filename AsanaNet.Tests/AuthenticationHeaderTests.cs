using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;
using FluentAssertions;
using System.Text;

namespace AsanaNet.Tests
{
    public class AuthenticationHeaderTests : TestBase
    {
        [Fact]
        public void BasicAuth_ShouldSetCorrectAuthorizationHeader()
        {
            // Arrange
            var expectedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{TestApiKey}:"));
            var expectedHeaderValue = new AuthenticationHeaderValue("Basic", expectedCredentials);

            // Act
            var client = new Asana(TestApiKey, AuthenticationType.Basic);

            // Assert
            var httpClient = GetHttpClientFromAsanaClient(client);
            httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Basic");
            httpClient.DefaultRequestHeaders.Authorization!.Parameter.Should().Be(expectedCredentials);
        }

        [Fact]
        public void OAuthAuth_ShouldSetCorrectAuthorizationHeader()
        {
            // Arrange
            var oAuthToken = "test_oauth_token";
            var expectedHeaderValue = new AuthenticationHeaderValue("Bearer", oAuthToken);

            // Act
            var client = new Asana(TestApiKey, AuthenticationType.OAuth, oAuthToken);

            // Assert
            var httpClient = GetHttpClientFromAsanaClient(client);
            httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            httpClient.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Bearer");
            httpClient.DefaultRequestHeaders.Authorization!.Parameter.Should().Be(oAuthToken);
        }

        [Fact]
        public void CustomHttpClient_WithExistingAuthHeader_ShouldNotOverrideHeader()
        {
            // Arrange
            var httpClient = new HttpClient();
            var customAuthHeader = new AuthenticationHeaderValue("Custom", "token");
            httpClient.DefaultRequestHeaders.Authorization = customAuthHeader;

            // Act
            var client = new Asana(TestApiKey, AuthenticationType.Basic, httpClient: httpClient);

            // Assert
            httpClient.DefaultRequestHeaders.Authorization.Should().Be(customAuthHeader);
        }

        private HttpClient GetHttpClientFromAsanaClient(Asana client)
        {
            // Use reflection to get the private _httpClient field
            var httpClientField = typeof(Asana).GetField("_httpClient", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (HttpClient)httpClientField!.GetValue(client)!;
        }
    }
} 