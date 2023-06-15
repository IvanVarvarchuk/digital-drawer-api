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
        public async Task<ActionResult> CreateApiKey([FromBody] CreateApiKeyCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet]
        public async Task<ActionResult> GetConvertion([FromRoute] GetApiKeysQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> GetConvertions([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new RevokeApiKeyCommand(){ Id = id }, cancellationToken));
        }
    }
}
