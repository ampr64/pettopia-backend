using Application.Features.Users;
using Application.Features.Users.Commands.CreateBackOfficeUser;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<string>> Create(CreateUserCommand command)
        {
            var id = await Mediator.Send(command);
            return Created($"api/users/{id}", id);
        }

        [HttpPost("backoffice-user")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<string>> CreateBackOfficeUser(CreateBackOfficeUserCommand command)
        {
            var id = await Mediator.Send(command);
            return Created($"api/users/backoffice-user/{id}", id);
        }

        [HttpPut("{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> UpdateUser(string userId, UpdateUserCommand command)
        {
            command.Id = userId;
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("backoffice-user/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> DeleteBackOfficeUser(string userId)
        {
            var command = new DeleteBackOfficeUserCommand
            {
                Id = userId
            };
            await Mediator.Send(command);

            return NoContent();
        }
    }
}