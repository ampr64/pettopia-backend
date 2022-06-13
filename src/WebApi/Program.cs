using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddWeb();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var dbContext = serviceProvider.GetRequiredService<PettopiaDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var dateTimeService = serviceProvider.GetRequiredService<IDateTimeService>();

    await PettopiaDbContextSeed.SeedAsync(dbContext, userManager, roleManager, dateTimeService, app.Logger);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();