using Application.Features.Authentication;
using Application.Features.Authentication.Commands.Authenticate;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<AuthenticateDto>> Authenticate(AuthenticateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}