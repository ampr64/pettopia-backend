using Application.Features.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
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
    }
}