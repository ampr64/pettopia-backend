using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where T : class
        {
            services.AddOptions<T>()
                .Bind(configuration.GetRequiredSection(sectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(provider => provider.GetRequiredService<IOptions<T>>().Value);
            
            return configuration.GetRequiredSection(sectionName).Get<T>();
        }
    }
}