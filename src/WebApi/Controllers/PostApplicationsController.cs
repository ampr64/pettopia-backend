using Application.Features.PostApplications.Commands.DeleteApplication;
using Application.Features.PostApplications.Commands.SubmitPostApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    [Route("api/posts")]
    public class PostApplicationsController : ApiControllerBase
    {
        [HttpPost("{postId:guid}/applications")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<string>> Create(Guid postId)
        {
            var id = await Mediator.Send(new SubmitPostApplicationCommand(postId));

            return Created(string.Empty, id);
        }

        [HttpDelete("{postId:guid}/applications/{id:guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> Delete([FromRoute] Guid postId, [FromRoute] Guid id)
        {
            await Mediator.Send(new DeletePostApplicationCommand(postId, id));

            return NoContent();
        }
    }
}