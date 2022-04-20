using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pettopia API", Version = "v1" });
                options.SupportNonNullableReferenceTypes();

                AddSecurityDefinition(options);
                AddSecurityRequirement(options);
            });

            services.AddFluentValidationRulesToSwagger();

            return services;
        }

        private static void AddSecurityDefinition(SwaggerGenOptions swaggerOpts)
        {
            swaggerOpts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Type into the textbox: Bearer {your JWT token}.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.ApiKey
            });
        }

        private static void AddSecurityRequirement(SwaggerGenOptions swaggerOpts)
        {
            swaggerOpts.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        }
    }
}