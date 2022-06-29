﻿using Application.Common.Interfaces;
using Application.Common.Settings;
using Azure.Storage.Blobs;
using Infrastructure.Extensions;
using Infrastructure.Mail;
using Infrastructure.Mail.Templates;
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

            services.AddIdentityCore<CustomIdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //options.SignIn.RequireConfirmedEmail = true;
            })
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

            var blobSettings = services.ConfigureSettings<BlobSettings>(configuration, BlobSettings.Section);
            services.ConfigureSettings<SmtpSettings>(configuration, SmtpSettings.Section);
            services.ConfigureSettings<AppClientUrlsSettings>(configuration, AppClientUrlsSettings.Section);

            services.AddSingleton(_ => new BlobServiceClient(blobSettings.ConnectionString));
            services.AddSingleton<IBlobService, AzureBlobService>();
            services.AddSingleton<IUriComposer, UriComposer>();

            services.AddScoped<IApplicationDbContext, PettopiaDbContext>();

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITokenClaimsService, IdentityTokenClaimsService>();
            services.AddTransient<IDateTimeService, UtcDateTimeService>();
            services.AddTransient<JwtSecurityTokenHandler>();
            services.AddTransient<IEmailService, SmtpEmailService>();
            services.AddTransient<IEmailTemplateService, EmailTemplateService>();

            return services;
        }
    }
}