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

        public async Task<Result<string?>> CreateUserAsync(string email, string password, string firstName, string lastName, DateTime birthDate, string role)
        {
            var applicationUser = new ApplicationUser(email, firstName, lastName, birthDate, _dateTimeService);

            var result = await _userManager.CreateAsync(applicationUser, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(applicationUser, role);
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

        public async Task<IReadOnlyList<UserInfo?>> GetUsersByRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            if (users is null)
            {
                var emptyList = new List<UserInfo>();
                return emptyList;
            }
           
            var result = IdentityExtensions.ToUserInfo(users, role);
            return result;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.DeleteAsync(user);
            
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(string userId, string email, string firstName, string lastName, DateTime birthDate)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.BirthDate = birthDate;
            var result = await _userManager.UpdateAsync(user);
            
            return result.Succeeded;
        }

        public async Task<UserInfo?> GetUserInfoByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }
            var result = IdentityExtensions.ToUserInfo(user);
            return result;
        }

        public async Task<bool> ValidateUserRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            bool isInRole = await _userManager.IsInRoleAsync(user, role);

            return isInRole;
        }

        public async Task<Result<string?>> CreateBackOfficeUserAsync(string email, string password, string firstName, string lastName, DateTime birthDate)
        {
            var applicationUser = new ApplicationUser(email, firstName, lastName, birthDate, _dateTimeService);

            var result = await _userManager.CreateAsync(applicationUser, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(applicationUser, Role.BackOfficeUser.Name);
            }

            return result.ToResultObject(applicationUser.Id);
        }
    }
}