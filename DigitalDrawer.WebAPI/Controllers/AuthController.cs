using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand;
using DigitalDrawer.Application.Features.Authentication.Command.Login;
using DigitalDrawer.Application.Features.Authentication.Command.Register;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AuthenticationResponse))]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result is null)
                {
                    return Unauthorized();
                }
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AuthenticationResponse))]
        public async Task<ActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result.Errors?.Length > 0)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
