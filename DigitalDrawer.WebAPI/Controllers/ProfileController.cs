using DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand;
using DigitalDrawer.Application.Features.Profile.Commands.UpdateProfileInfoCommand;
using DigitalDrawer.Application.Features.Profile.Queries.GetProfileInfoQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace DigitalDrawer.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class ProfileController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ProfileInfoDto))]
        public async Task<ActionResult> GetProfileInfo()
        {
            return Ok(await Mediator.Send(new GetProfileInfoQuery()));
        }
        
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProfileInfo(UpdateProfileInfoCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
