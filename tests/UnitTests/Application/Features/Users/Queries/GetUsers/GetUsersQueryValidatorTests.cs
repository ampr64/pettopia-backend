using Application.Features.Users.Queries;
using Application.Features.Users.Queries.GetUsers;
using FluentValidation.TestHelper;
using UnitTests.Configuration;
using Xunit;

namespace UnitTests.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryValidatorTests
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenRoleIsEmpty(string? role, GetUsersQueryValidator sut, GetUsersQuery query)
        {
            query.Role = role!;
            var actual = sut.TestValidate(query);

            actual.ShouldHaveValidationErrorFor(c => c.Role)
                .WithErrorCode("NotEmptyValidator");
        }
    }
}
