using Microsoft.Extensions.DependencyInjection;
using System;
using AsanaNet.Options;

namespace AsanaNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAsanaNet(
            this IServiceCollection services, 
            Action<AsanaOptions> configureOptions)
        {
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            var options = new AsanaOptions();
            configureOptions(options);

            if (string.IsNullOrEmpty(options.ApiKey))
                throw new ArgumentException("API key cannot be empty", nameof(configureOptions));

            services.Configure(configureOptions);
            
            services.AddSingleton<IAsanaClient>(sp =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://app.asana.com/api/1.0/")
                };
                return new Asana(options.ApiKey, options.AuthenticationType);
            });

            return services;
        }
    }
} 