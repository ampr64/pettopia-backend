using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities.Users;
using Domain.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public IdentityService(IApplicationDbContext dbContext,
            UserManager<CustomIdentityUser> userManager,
            SignInManager<CustomIdentityUser> signInManager,
            ITokenClaimsService tokenClaimsService,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _currentUserService = currentUserService;
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

        public async Task<Result<string?>> CreateUserAsync(string email, string password, Role role, CancellationToken cancellationToken = default)
        {
            // TODO: Send confirmation email
            var user = new CustomIdentityUser(email);

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, role.Name);
            }

            return result.ToResultObject(user.Id);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return true;
            }
            
            var result = await _userManager.SetLockoutEnabledAsync(user, true);
            
            if (result.Succeeded)
            {
                result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }

            return result.Succeeded;
        }

        public async Task<bool> ChangeEmailAsync(string userId, string email)
        {
            // TODO: Send token for email confirmation
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }

            user.Email = email;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }

        public async Task<UserInfo?> GetUserInfoByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            return new UserInfo();
        }

        public async Task<bool> IsElevatedAccessUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await GetUserRoleAsync(user);
            var elevatedRoles = new[] { Role.Admin.Name, Role.BackOfficeUser.Name };

            return elevatedRoles.Contains(role);
        }

        private async Task<string> GetUserRoleAsync(CustomIdentityUser user)
        {
            return (await _userManager.GetRolesAsync(user)).Single();
        }

        public async Task<Member?> GetCurrentUserAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            if (_currentUserService.UserId is null)
            {
                return null;
            }

            var query = (trackChanges ? _dbContext.Members : _dbContext.Members.AsNoTracking());

            return await query.IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == _currentUserService.UserId, cancellationToken);
        }

        public async Task<string?> GetEmailConfirmationToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user is null ? null : await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<UserInfo?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUserService.UserId is null)
            {
                return null;
            }

            return await GetUserInfoByIdAsync(_currentUserService.UserId);
        }
    }
}