using AutoFixture;
using AutoFixture.AutoMoq;

namespace UnitTests.Configuration.Identity
{
    public class IdentityConventions : CompositeCustomization
    {
        public IdentityConventions() : base(
            new AutoMoqCustomization { ConfigureMembers = true },
            new UserManagerCustomization())
        {
        }
    }
}