using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace UnitTests.Configuration.Identity
{
    public class IdentityAutoDataAttribute : AutoDataAttribute
    {
        public IdentityAutoDataAttribute() : base(() =>
        {
            var fixture = new Fixture();
            fixture.Customize(new IdentityConventions());
            fixture.Customizations.Add(new TypeRelay(typeof(SecurityToken), typeof(JwtSecurityToken)));

            return fixture;
        })
        {
        }
    }
}