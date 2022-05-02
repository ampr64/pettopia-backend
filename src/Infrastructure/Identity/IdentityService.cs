using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enumerations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IDateTimeService _dateTimeService;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenClaimsService tokenClaimsService,
            IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
            _dateTimeService = dateTimeService;
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result.Succeeded ? (await _tokenClaimsService.GetTokenAsync(email)) : null;
        }

        public async Task<Result<string?>> CreateUserAsync(string email, string password, string firstName, string lastName, DateTime birthDate)
        {
            var applicationUser = new ApplicationUser(email, firstName, lastName, birthDate, _dateTimeService);

            var result = await _userManager.CreateAsync(applicationUser, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(applicationUser, Role.User.Name);
            }

            return result.ToResultObject(applicationUser.Id);
        }

        public async Task<UserInfo?> GetUserInfoAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var role = (await _userManager.GetRolesAsync(user)).Single();
            return user.ToUserInfo(role);
        }
    }
}