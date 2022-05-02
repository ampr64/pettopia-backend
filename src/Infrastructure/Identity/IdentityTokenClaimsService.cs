using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity
{
    public class IdentityTokenClaimsService : ITokenClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsPrincipalFactory;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfiguration _configuration;

        public IdentityTokenClaimsService(UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsPrincipalFactory,
            JwtSecurityTokenHandler tokenHandler,
            IDateTimeService dateTimeService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _tokenHandler = tokenHandler;
            _dateTimeService = dateTimeService;
            _configuration = configuration;
        }

        public async Task<string?> GetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return null;
            }

            var secret = _configuration.GetValue<string>("AppSecret");
            var key = Encoding.ASCII.GetBytes(secret);
            var principal = await _claimsPrincipalFactory.CreateAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(principal.Claims),
                Expires = _dateTimeService.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}