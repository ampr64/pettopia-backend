using Application.Common.Interfaces;
using FluentValidation.AspNetCore;
using WebApi.Configurations;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi
{
    public static class DependenciesExtensions
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services
                .AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation();

            services.AddEndpointsApiExplorer();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.ConfigureSwagger();

            services.AddHttpContextAccessor();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}