using Application.Common.Interfaces;
using Domain.Enumerations;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class PettopiaDbContextSeed
    {
        public static async Task SeedAsync(PettopiaDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IDateTimeService dateTimeService,
            ILogger logger,
            int retryLimit = 5)
        {
            try
            {
                if (dbContext.Database.IsSqlServer())
                {
                    await dbContext.Database.MigrateAsync();
                }

                await SeedDefaultRolesAsync(roleManager);
                await SeedDefaultUsersAsync(userManager, dateTimeService);

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (retryLimit == 0) throw;

                retryLimit--;

                logger.LogError(ex, "An error occurred while migrating or seeding the database, retrying...");
                await SeedAsync(dbContext, userManager, roleManager, dateTimeService, logger, retryLimit);
            }
        }

        private static async Task SeedDefaultRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in Role.List)
                {
                    var identityRole = new IdentityRole(role.Name)
                    {
                        Id = role.Value
                    };

                    await roleManager.CreateAsync(identityRole);
                }
            }
        }

        private static async Task SeedDefaultUsersAsync(UserManager<ApplicationUser> userManager, IDateTimeService dateTimeService)
        {
            var defaultUser = new ApplicationUser("admin@petutopia.com", "Admin", "Admin", new DateTime(1990, 1, 1), dateTimeService);

            if (!await userManager.Users.AnyAsync(u => u.UserName == defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Admin123!");
                await userManager.AddToRoleAsync(defaultUser, Role.Admin.Name);
            }
        }
    }
}