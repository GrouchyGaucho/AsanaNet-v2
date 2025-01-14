using Microsoft.Extensions.DependencyInjection;
using AsanaNet.Options;

namespace AsanaNet.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAsanaNet(
        this IServiceCollection services, 
        Action<AsanaOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddHttpClient<IAsanaClient, Asana>();
        return services;
    }
} 