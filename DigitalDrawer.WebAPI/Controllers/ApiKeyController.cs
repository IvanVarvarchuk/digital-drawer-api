using DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand;
using DigitalDrawer.Application.Features.ApiKey.Commands.RevokeApiKeyCommand;
using DigitalDrawer.Application.Features.ApiKey.Queries.GetApiKeysQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace DigitalDrawer.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiKeyController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateApiKey([FromBody] CreateApiKeyCommand command, CancellationToken cancellationToken)
        {
            var apiKey = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetApiKeyById), new { id = apiKey.Id }, apiKey);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetApiKeys()
        {
            return Ok(await Mediator.Send(new GetApiKeysQuery()));
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetApiKeyById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new RevokeApiKeyCommand(){ Id = id }, cancellationToken));
        }
    }
}
