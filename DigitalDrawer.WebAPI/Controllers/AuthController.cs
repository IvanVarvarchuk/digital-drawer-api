using DigitalDrawer.Application.Features.Authorization.Command.Login;
using DigitalDrawer.Application.Features.Authorization.Command.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDrawer.WebAPI.Controllers
{
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        [HttpPost]
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
        public async Task<ActionResult> Register([FromBody] RegisterCommand command)
        {
            return Created("", await Mediator.Send(command));
        }
    }
}
