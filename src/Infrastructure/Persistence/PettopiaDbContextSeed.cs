using Application.Common.Interfaces;
using Domain.Entities.Users;
using Domain.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public class PettopiaDbContextSeed
    {
        public static async Task SeedAsync(PettopiaDbContext dbContext,
            UserManager<CustomIdentityUser> userManager,
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
                await SeedDefaultUsersAsync(dbContext, userManager, dateTimeService);

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
            var dbRoles = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            var missingRoles = Role.List.Select(r => r.Name).Except(dbRoles).ToList();

            foreach (var role in missingRoles)
            {
                var identityRole = new IdentityRole(role);
                await roleManager.CreateAsync(identityRole);
            }
        }

        private static async Task SeedDefaultUsersAsync(PettopiaDbContext dbContext,
            UserManager<CustomIdentityUser> userManager,
            IDateTimeService dateTimeService)
        {
            var admin = new CustomIdentityUser("admin@pettopia.com")
            {
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            var adminUser = new Administrator(admin.Id, "Manuel", "Sadosky", admin.Email, new DateTime(1990, 12, 12), dateTimeService.Now);

            if (!await userManager.Users.AnyAsync(u => u.Email == admin.Email))
            {
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, Role.Admin.Name);
            }

            if (!await dbContext.Members.IgnoreQueryFilters().AnyAsync(m => m.Email == admin.Email))
            {
                await dbContext.AddAsync(adminUser);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}