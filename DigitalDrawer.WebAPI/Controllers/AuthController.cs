using DigitalDrawer.Application.Features.Authentication.Command.Login;
using DigitalDrawer.Application.Features.Authentication.Command.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDrawer.WebAPI.Controllers
{
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await Mediator.Send(command);
            if (result == null)
            {
                return Unauthorized();
            }
            return Accepted(result);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await Mediator.Send(command); 
            if (result.Errors.Length > 0)
            {
                return BadRequest(result);
            }

            return Created("", await Mediator.Send(command));
        }
    }
}
