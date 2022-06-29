using Application.Features.Authentication;
using Application.Features.Authentication.Commands.Authenticate;
using Application.Features.Authentication.Commands.ConfirmEmail;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<AuthenticateDto>> Authenticate(AuthenticateCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }
    }
}