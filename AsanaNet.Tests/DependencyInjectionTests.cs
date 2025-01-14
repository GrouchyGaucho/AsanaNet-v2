using System;
using Microsoft.Extensions.DependencyInjection;
using AsanaNet.Extensions;
using AsanaNet.Options;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace AsanaNet.Tests;

public class DependencyInjectionTests
{
    private const string TestApiKey = "test_api_key";
    private const string TestOAuthToken = "test_oauth_token";

    [Fact]
    public void AddAsanaNet_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAsanaNet(options =>
        {
            options.ApiKey = TestApiKey;
            options.AuthenticationType = AuthenticationType.Basic;
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var client = serviceProvider.GetService<IAsanaClient>();
        client.Should().NotBeNull();
        client.Should().BeOfType<Asana>();
    }

    [Fact]
    public void AddAsanaNet_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        services.Invoking(s => s.AddAsanaNet(null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddAsanaNet_WithEmptyApiKey_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        services.Invoking(s => s.AddAsanaNet(options =>
        {
            options.ApiKey = string.Empty;
        }))
        .Should().Throw<ArgumentException>()
        .WithMessage("*API key cannot be empty*");
    }

    [Fact]
    public void AddAsanaNet_ShouldConfigureHttpClient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddAsanaNet(options =>
        {
            options.ApiKey = TestApiKey;
            options.AuthenticationType = AuthenticationType.Basic;
        });

        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<IAsanaClient>();

        // Assert
        client.Should().NotBeNull();
        var asanaClient = client as Asana;
        asanaClient.Should().NotBeNull();
        asanaClient!.ApiKey.Should().Be(TestApiKey);
        asanaClient.AuthType.Should().Be(AuthenticationType.Basic);
    }

    [Fact]
    public void AddAsanaNet_ShouldRegisterAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        var apiKey = "test_api_key";

        // Act
        services.AddAsanaNet(options =>
        {
            options.ApiKey = apiKey;
        });

        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAsanaClient));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);

        var client1 = serviceProvider.GetRequiredService<IAsanaClient>();
        var client2 = serviceProvider.GetRequiredService<IAsanaClient>();

        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        client1.Should().BeSameAs(client2); // Verify it's the same instance
        
        // Verify properties
        client1.ApiKey.Should().Be(apiKey);
        client1.BaseUrl.Should().Be("https://app.asana.com/api/1.0/");
        client1.AuthType.Should().Be(AuthenticationType.Basic);
        client1.OAuthToken.Should().BeNull();
    }
} 