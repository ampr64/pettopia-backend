using Application.Features.Stats.Queries.GetStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin,BackOfficeUser")]
    public class StatsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<StatsVm>> Get([FromQuery] GetStatsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}