using AutoFixture;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using UnitTests.TestExtensions;

namespace UnitTests.Configuration.Identity
{
    internal class UserManagerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.FreezeMoq(() => new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserEmailStore<ApplicationUser>>(), null, null, null, null, null, null, null, null));
        }
    }
}