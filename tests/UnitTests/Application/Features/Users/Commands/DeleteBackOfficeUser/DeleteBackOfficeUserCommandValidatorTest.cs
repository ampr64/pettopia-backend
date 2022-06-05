using Application.Features.Users.Commands.DeleteUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteBackOfficeUserCommandValidatorTest
    {
        [Theory]
        [AutoMoqInlineData(null!)]
        [AutoMoqInlineData("")]
        [AutoMoqInlineData("   ")]
        public void Validate_ShouldHaveError_WhenIdIsEmpty(string? id, DeleteBackOfficeUserCommandValidator sut, DeleteBackOfficeUserCommand command)
        {
            command.Id = id!;
            var actual = sut.TestValidate(command);

            actual.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorCode("NotEmptyValidator");
        }
    }
}
