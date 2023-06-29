using DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand;
using DigitalDrawer.Application.Features.ApiKey.Commands.RevokeApiKeyCommand;
using DigitalDrawer.Application.Features.ApiKey.Queries.GetApiKeysQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace DigitalDrawer.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiKeyController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CreateApiKeyCommandDto))]
        public async Task<ActionResult> CreateApiKey([FromBody] CreateApiKeyCommand command, CancellationToken cancellationToken)
        {
            var apiKey = await Mediator.Send(command, cancellationToken);
            return Ok(apiKey);
            //return CreatedAtAction(nameof(GetApiKeys), apiKey);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<CreateApiKeyCommandDto>))]
        public async Task<ActionResult> GetApiKeys()
        {
            return Ok(await Mediator.Send(new GetApiKeysQuery()));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteApiKeyById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new RevokeApiKeyCommand(){ Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
