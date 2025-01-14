using System;
using System.Net.Http;
using Xunit;
using FluentAssertions;

namespace AsanaNet.Tests
{
    public class AuthenticationTests
    {
        private const string TestApiKey = "test_api_key";
        private const string TestOAuthToken = "test_oauth_token";

        [Fact]
        public void Constructor_WithBasicAuth_ShouldConfigureBasicAuthentication()
        {
            // Act
            var client = new Asana(TestApiKey, AuthenticationType.Basic);

            // Assert
            client.ApiKey.Should().Be(TestApiKey);
            client.AuthType.Should().Be(AuthenticationType.Basic);
            client.OAuthToken.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithOAuth_ShouldConfigureOAuthAuthentication()
        {
            // Act
            var client = new Asana(TestApiKey, AuthenticationType.OAuth, TestOAuthToken);

            // Assert
            client.ApiKey.Should().Be(TestApiKey);
            client.AuthType.Should().Be(AuthenticationType.OAuth);
            client.OAuthToken.Should().Be(TestOAuthToken);
        }

        [Fact]
        public void Constructor_WithOAuthAndNoToken_ShouldThrowArgumentException()
        {
            // Act & Assert
            Action act = () => new Asana(TestApiKey, AuthenticationType.OAuth);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*OAuth token is required*");
        }

        [Fact]
        public void Constructor_WithNullApiKey_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Action act = () => new Asana(null!);
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("apiKey");
        }

        [Fact]
        public void Constructor_WithCustomHttpClient_ShouldUseProvidedClient()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var client = new Asana(TestApiKey, AuthenticationType.Basic, httpClient: httpClient);

            // Assert
            client.ApiKey.Should().Be(TestApiKey);
            client.AuthType.Should().Be(AuthenticationType.Basic);
        }

        [Fact]
        public void Constructor_WithUnsupportedAuthType_ShouldThrowArgumentException()
        {
            // Arrange
            var unsupportedAuthType = (AuthenticationType)999;

            // Act & Assert
            Action act = () => new Asana(TestApiKey, unsupportedAuthType);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Unsupported authentication type*");
        }
    }
} 