using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Mail;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure
{
    public static class DependenciesExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PettopiaDbContext>(dbOptions =>
            {
                dbOptions.UseSqlServer(
                    configuration.GetConnectionString(PettopiaDbContext.ConnectionStringKey),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(PettopiaDbContext).Assembly.FullName));
            });

            services.AddIdentityCore<ApplicationUser>(options => options.User.RequireUniqueEmail = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PettopiaDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();

            services
                .AddAuthentication(authOpts => authOpts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOpts =>
                {
                    jwtOpts.SaveToken = true;

                    var secret = configuration.GetValue<string>("AppSecret");
                    var key = Encoding.ASCII.GetBytes(secret);
                    jwtOpts.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true
                    };
                });

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenClaimsService, IdentityTokenClaimsService>();
            services.AddTransient<IDateTimeService, UtcDateTimeService>();
            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<IEmailService, SmtpEmailService>();

            return services;
        }
    }
}